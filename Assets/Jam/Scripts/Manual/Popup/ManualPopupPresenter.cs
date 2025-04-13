using Zenject;

namespace Jam.Scripts.Manual.Popup
{
    public class ManualPopupPresenter
    {
        [Inject] private ManualPagesRepository _repository;

        private ManualPopup _view;

        public void AttachView(ManualPopup view)
        {
            if (_view != null) return;
            _view = view;
            SetPages();
        }

        private void SetPages()
        {
            var pages = _repository.Definitions;
            _view.SetPages(pages);
        }
    }
}