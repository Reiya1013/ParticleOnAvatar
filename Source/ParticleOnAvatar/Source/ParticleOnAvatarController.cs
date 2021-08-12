using ParticleOnAvatar.Views;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleOnAvatar
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class ParticleOnAvatarController : MonoBehaviour
    {
        public static ParticleOnAvatarController Instance { get; private set; }
        private ModMainFlowCoordinator mainFlowCoordinator;
        internal static string Name => "ParticleOnAvatar";

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            if (Instance != null)
            {
                Logger.log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;
            Logger.log?.Debug($"{name}: Awake()");

            //AvatarDanceMenu
            MenuButton menuButton = new MenuButton("Particle On Avatar", "scribble mod", ShowModFlowCoordinator, true);
            MenuButtons.instance.RegisterButton(menuButton);

        }

        /// <summary>
        /// アバターダンスメニューコントローラー
        /// </summary>
        public void ShowModFlowCoordinator()
        {
            if (this.mainFlowCoordinator == null)
                this.mainFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModMainFlowCoordinator>();
            if (mainFlowCoordinator.IsBusy) return;

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(mainFlowCoordinator);
        }

        /// <summary>
        /// Only ever called once on the first frame the script is Enabled. Start is called after any other script's Awake() and before Update().
        /// </summary>
        private void Start()
        {

        }

        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Called every frame after every other enabled script's Update().
        /// </summary>
        private void LateUpdate()
        {

        }

        /// <summary>
        /// Called when the script becomes enabled and active
        /// </summary>
        private void OnEnable()
        {

        }

        /// <summary>
        /// Called when the script becomes disabled or when it is being destroyed.
        /// </summary>
        private void OnDisable()
        {

        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Logger.log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.

        }
        #endregion
    }
}
