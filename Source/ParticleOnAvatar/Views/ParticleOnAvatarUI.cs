using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using System.IO;
using ParticleOnAvatar.Parameter;

namespace ParticleOnAvatar.Views
{
    class ParticleOnAvatarUI : BSMLAutomaticViewController
    {
        public ModMainFlowCoordinator mainFlowCoordinator { get; set; }
        public void SetMainFlowCoordinator(ModMainFlowCoordinator mainFlowCoordinator)
        {
            this.mainFlowCoordinator = mainFlowCoordinator;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        [UIComponent("ParticleList")]
        private CustomListTableData particleNameList = new CustomListTableData();
        private int selectRow;
        [UIAction("ParticleSelect")]
        private void Select(TableView _, int row)
        {
            selectRow = row;
        }


        [UIAction("SetParticle")]
        private void SetParticle()
        {
            Plugin.Particle.ParticleDestroy();
            if (selectRow - 1 >= 0)
                SharedCoroutineStarter.instance.StartCoroutine(Plugin.Particle.GetVRMAndSetParticle(selectRow-1));
        }

        [UIAction("Clear")]
        private void Clear()
        {
            Plugin.Particle.ParticleDestroy();
        }



        [UIAction("#post-parse")]
        public void SetupList()
        {

            particleNameList.data.Clear();
            particleNameList.data.Add(new CustomListTableData.CustomCellInfo("Null"));
            foreach (var materialName in Plugin.Particle.GetParticleName())
            {
                var customCellInfo = new CustomListTableData.CustomCellInfo(materialName);
                particleNameList.data.Add(customCellInfo);
            }

            particleNameList.tableView.ReloadData();
        }
    }
}
