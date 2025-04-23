using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualPopup : Popup
    {
        [SerializeField] private Transform _leftPageHolder;
        [SerializeField] private Transform _rightPageHolder;
        [Header("Navigation buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private Button _nextPageButton;
        [Header("Bookmarks")] 
        [SerializeField] private Transform _leftBookmarksHolder;
        [SerializeField] private Transform _rightBookmarksHolder;
        
        [SerializeField] private List<Bookmark> _rightBookmarks;
        [SerializeField] private List<Bookmark> _leftBookmarks;

        [Inject] private ManualPagesRepository _pagesRepository;
        [Inject] private ReagentRepository _reagentRepository;
        [Inject] private AudioService _audioService;

        private List<Page> _pages;
        private int _currentPageIndex = 0;

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

        public override void Close()
        {
            _audioService.PlaySound(Sounds.manualClose.ToString());
            base.Close();
        }

        public void Initialize(List<ReagentExclusion> reagentExclusions)
        {
            _pages.ForEach(page =>
            {
                page.Initialize(reagentExclusions);
            });
        }

        private void InitPages()
        {
            _pages = new List<Page>();
            for (int i = 0; i < _pagesRepository.Definitions.Count; i++)
            {
                Page pageObject = Instantiate(_pagesRepository.Definitions[i].Page, i % 2 == 0 ? _leftBookmarksHolder : _rightBookmarksHolder);
                _pages.Add(pageObject);
            }
            ShowPages();
        }

        private void ShowPages()
        {
            RenderNavigateButtons();
            RenderBookmarks();

            if (_pages.Count == 1)
            {
                _pages[_currentPageIndex].gameObject.SetActive(true);
                return;
            }

            if (_currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].gameObject.SetActive(true);
            }

            if (_currentPageIndex + 1 < _pages.Count)
            {
                _pages[_currentPageIndex + 1].gameObject.SetActive(true);
            }
        }

        private void RenderBookmarks()
        {
            _leftBookmarks.ForEach(bookmark =>
            {
                bookmark.gameObject.SetActive(_currentPageIndex >= bookmark.PageIndex);
                if (bookmark.PageIndex == _currentPageIndex)
                {
                    bookmark.transform.SetAsLastSibling();
                }
                else
                {
                    bookmark.transform.SetAsFirstSibling();
                }
            });
            _rightBookmarks.ForEach(bookmark =>
            {
                bookmark.gameObject.SetActive(_currentPageIndex + 1 <= bookmark.PageIndex);
                if (bookmark.PageIndex == _currentPageIndex + 1)
                {
                    bookmark.transform.SetAsLastSibling();
                }
                else
                {
                    bookmark.transform.SetAsFirstSibling();
                }
            });
        }

        private void RenderNavigateButtons()
        {
            var isNextButtonVisible = _currentPageIndex + 2 < _pages.Count;
            var isPreviousButtonVisible = _currentPageIndex != 0;
            _nextPageButton.gameObject.SetActive(isNextButtonVisible);
            _prevPageButton.gameObject.SetActive(isPreviousButtonVisible);
        }

        private void PrevPageClick()
        {
            _audioService.PlaySound(Sounds.bookFlip.ToString());
            HideCurrentPages();
            _currentPageIndex -= 2;
            ShowPages();
        }

        private void NextPageClick()
        {
            _audioService.PlaySound(Sounds.bookFlip.ToString());
            HideCurrentPages();
            _currentPageIndex += 2;
            ShowPages();
        }

        private void HideCurrentPages()
        {
            _pages[_currentPageIndex].gameObject.SetActive(false);
            if (_currentPageIndex + 1 < _pages.Count)
            {
                _pages[_currentPageIndex + 1].gameObject.SetActive(false);
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