using BeatSaberMarkupLanguage;
using HMUI;

namespace ParticleOnAvatar.Views
{
    class ModMainFlowCoordinator : FlowCoordinator
    {
        private const string titleString = "Particle On Avatar";
        private ParticleOnAvatarUI avatarDanceUI;
        
        public bool IsBusy { get; set; }

        public void ShowUI()
        {
            this.IsBusy = true;
            this.SetLeftScreenViewController(this.avatarDanceUI, ViewController.AnimationType.In);
            this.IsBusy = false;
        }

        private void Awake()
        {
            this.avatarDanceUI = BeatSaberUI.CreateViewController<ParticleOnAvatarUI>();
            this.avatarDanceUI.mainFlowCoordinator = this;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle(titleString);
            this.showBackButton = true;

            var viewToDisplay = DecideMainView();

            this.IsBusy = true;
            ProvideInitialViewControllers(viewToDisplay);
            this.IsBusy = false;
        }

        private ViewController DecideMainView()
        {
            ViewController viewToDisplay;

            viewToDisplay = this.avatarDanceUI;

            return viewToDisplay;
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            if (this.IsBusy) return;

            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
