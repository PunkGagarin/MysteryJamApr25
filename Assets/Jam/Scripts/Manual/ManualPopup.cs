using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualPopup : Popup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Transform _leftPageHolder;
        [SerializeField] private Transform _rightPageHolder;
        [Inject] private ManualPagesRepository _repository;

        private List<GameObject> _pages;
        private int _currentPage = 0;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
            _prevPageButton.onClick.AddListener(PrevPageClick);
            _nextPageButton.onClick.AddListener(NextPageClick);
        }

        public override void Open(bool withPause)
        {
            base.Open(withPause);
            if (_pages == null) InitPages();
            else ShowPages();
        }

        private void InitPages()
        {
            var pages = _repository.Definitions;
            _pages = new List<GameObject>();
            foreach (var page in pages.Select((value, index) => new { value, index }))
            {
                var holder = (page.index + 1) switch
                {
                    1 when pages.Count == 1 => _rightPageHolder,
                    var i when i % 2 == 0 => _rightPageHolder,
                    _ => _leftPageHolder
                };
                var pageObject = Instantiate(page.value.PagePrefab, holder);
                pageObject.gameObject.SetActive(false);
                
                _pages.Add(pageObject);
            }
            ShowPages();
        }

        private void ShowPages()
        {
            RenderNavigateButtons();

            if (_pages.Count == 1)
            {
                _pages[_currentPage].gameObject.SetActive(true);
                return;
            }

            if (_currentPage < _pages.Count)
            {
                _pages[_currentPage].SetActive(true);
            }

            if (_currentPage + 1 < _pages.Count)
            {
                _pages[_currentPage + 1].SetActive(true);
            }
        }

        private void RenderNavigateButtons()
        {
            var isNextButtonVisible = _currentPage + 2 < _pages.Count;
            var isPreviousButtonVisible = _currentPage != 0;
            _nextPageButton.gameObject.SetActive(isNextButtonVisible);
            _prevPageButton.gameObject.SetActive(isPreviousButtonVisible);
        }

        private void PrevPageClick()
        {
            HideCurrentPages();
            _currentPage -= 2;
            ShowPages();
        }

        private void NextPageClick()
        {
            HideCurrentPages();
            _currentPage += 2;
            ShowPages();
        }

        private void HideCurrentPages()
        {
            _pages[_currentPage].SetActive(false);
            if (_currentPage + 1 < _pages.Count)
            {
                _pages[_currentPage + 1].SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _prevPageButton.onClick.RemoveListener(PrevPageClick);
            _nextPageButton.onClick.RemoveListener(NextPageClick);
        }
    }
}