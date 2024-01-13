using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ITilePresenter))]
    public class TileSlotUI : MonoBehaviour
    {
        [SerializeField] private TileSlot tileSlot;
        [SerializeField] private ITilePresenter _tilePresenter;

        private void Awake()
        {
            tileSlot = GetComponent<TileSlot>();
            _tilePresenter = GetComponent<ITilePresenter>();
        }

        private void Start()
        {
            DisplayTile();
        }

        private void DisplayTile()
        {
            _tilePresenter.PresentTile(tileSlot.GetTile());
        }
    }
}