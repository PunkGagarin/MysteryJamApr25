using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Manual.Popup
{
    public class ManualPopup : Utils.UI.Popup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Transform _leftPageHolder;
        [SerializeField] private Transform _rightPageHolder;
        
        private ManualPopupPresenter _presenter;
        private List<ManualPage> _pages;
        private int _currentPage = 0;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
            _prevPageButton.onClick.AddListener(PrevPageClick);
            _nextPageButton.onClick.AddListener(NextPageClick);
        }

        public void SetPresenter(ManualPopupPresenter presenter)
        {
            _presenter = presenter;
            _presenter.AttachView(this);
        }

        public void SetPages(List<ManualPage> pages)
        {
            _pages = pages;
            ShowPages();
        }

        private void ShowPages()
        {
            DestroyCurrentPages();

            RenderNavigateButtons();
            
            if (_pages.Count == 1)
            {
                RenderFirstSinglePage();
                return;
            }

            if (_currentPage < _pages.Count)
            {
                Instantiate(_pages[_currentPage].PagePrefab, _leftPageHolder);
            }

            if (_currentPage + 1 < _pages.Count)
            {
                Instantiate(_pages[_currentPage + 1].PagePrefab, _rightPageHolder);
            }
        }

        private void RenderNavigateButtons()
        {
            var isSinglePage = _pages.Count == 1;
            var isFirstPage = _currentPage == 0;
            var hasNextPairPage = _currentPage + 2 < _pages.Count;
            var isNextButtonVisible = !isSinglePage && hasNextPairPage;
            var isPreviousButtonVisible = !isSinglePage && !isFirstPage;
            _nextPageButton.gameObject.SetActive(isNextButtonVisible);
            _prevPageButton.gameObject.SetActive(isPreviousButtonVisible);
        }

        private void RenderFirstSinglePage()
        {
            Instantiate(_pages[_currentPage].PagePrefab, _rightPageHolder);
            _nextPageButton.gameObject.SetActive(false);
            _prevPageButton.gameObject.SetActive(false);
        }

        private void PrevPageClick()
        {
            if (_currentPage - 2 >= 0)
            {
                _currentPage -= 2;
                ShowPages();
            }
        }

        private void NextPageClick()
        {
            if (_currentPage + 2 <= _pages.Count)
            {
                _currentPage += 2;
                ShowPages();
            }
        }
        
        private void DestroyCurrentPages()
        {
            foreach (Transform child in _leftPageHolder) Destroy(child.gameObject);
            foreach (Transform child in _rightPageHolder) Destroy(child.gameObject);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _prevPageButton.onClick.RemoveListener(PrevPageClick);
            _nextPageButton.onClick.RemoveListener(NextPageClick);
        }
    }
}