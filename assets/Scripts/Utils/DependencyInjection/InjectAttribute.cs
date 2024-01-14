using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Utils.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    { }
    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute
    { }
    
    public interface IDependencyProvider { }

    public class Injector : Singleton<Injector>
    {
        private const BindingFlags KBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new ();

        protected override void Awake()
        {
            base.Awake();

            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        private void Inject(MonoBehaviour injectable)
        {
            var type = injectable.GetType();
            
            // Inject Fields
            var injectableFields = type.GetFields(KBindingFlags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to resolve {fieldType.Name} for {type.Name}");
                }
                
                injectableField.SetValue(injectable, resolvedInstance);
            }
            
            // Inject Methods
            var injectableMethods = type.GetMethods(KBindingFlags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            
            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Failed to resolve {type.Name}.{injectableMethod.Name}");
                }
                
                injectableMethod.Invoke(injectable, resolvedInstances);
            }
        }

        private object Resolve(Type type)
        {
            _registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }


        static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(KBindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
        
        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(KBindingFlags);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)));

                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    _registry.Add(returnType, providedInstance);
                }
                else
                {
                    throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
                }
            }
        }
        
        private static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsOfType<MonoBehaviour>();
        }
    }
}