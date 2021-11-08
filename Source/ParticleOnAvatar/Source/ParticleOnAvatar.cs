using ParticleOnAvatar.Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using VRM;

namespace ParticleOnAvatar
{
    class ParticleOnAvatar : MonoBehaviour
    {
        UnityEngine.Object[] AssetsParticle;
        GameObject ParticleObject;
        private GameObject VRM;
        private List<string> ParticleName = new List<string>();


        private AssetBundle assetBundle;

        /// <summary>
        /// 変更Particleファイル一覧取得
        /// </summary>
        /// <returns></returns>
        public string[] GetParticleName()
        {
            ParticleName.Clear();
            string[] names = Directory.GetFiles($"{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\ParticleOnAvatar"}", "*.particle");
            string[] rtnNames = new string[names.Length];
            for (int i = 0; i < rtnNames.Length; i++)
            {
                rtnNames[i] = Path.GetFileName(names[i]);
                ParticleName.Add(names[i]);
            }

            return rtnNames;
        }

        /// <summary>
        /// アセットバンドルを読み込んで保持しておく
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="set"></param>
        private IEnumerator ParticleLoad(string filename)
        {
            if (!ParticleObject) GameObject.Destroy(ParticleObject);
            if (filename == "") yield break;
            //ロード済みならアンロードする
            if (assetBundle) assetBundle.Unload(true);

            Logger.log?.Debug($"ParticleLoad {filename}");
            var asyncLoad = AssetBundle.LoadFromFileAsync(filename);
            yield return asyncLoad;
            assetBundle = asyncLoad.assetBundle;
            var assetLoadRequest = assetBundle.LoadAllAssetsAsync();
            yield return assetLoadRequest;
            AssetsParticle = assetLoadRequest.allAssets;

            foreach (var asset in AssetsParticle)
            {
                if (asset is GameObject gameObject)
                {
                    ParticleObject = gameObject;
                    break;
                }
            }

        }

        bool SetupFlg = false;
        /// <summary>
        /// VRM取得して、Danceモーションのセット
        /// </summary>
        public IEnumerator GetVRMAndSetParticle(int selectNo)
        {
            //選択したDanceをロードする
            yield return ParticleLoad(ParticleName[selectNo]);

            //読み込み失敗で終了
            if (ParticleObject == null) yield break;

            //VRMなくても終了
            VRM = GameObject.Find("VRM");
            if (VRM == null) yield break;

            SetParticle();
        }

        public void LateUpdate()
        {

        }

        /// <summary>
        /// 設定済みのParticleをDestroyする
        /// </summary>
        public void ParticleDestroy()
        {
            foreach (var obj in SetParticles)
                GameObject.Destroy(obj);

            SetParticles.Clear();
        }


        //削除用に元を取っておく
        private List<GameObject> SetParticles = new List<GameObject>();

        /// <summary>
        /// Particleをセットする
        /// </summary>
        private void SetParticle()
        {
            Animator ani = VRM.GetComponent<Animator>();

            var leftHand = ParticleObject.transform.Find("LeftHand");
            if (leftHand)
                SetRigParticle(ani, HumanBodyBones.LeftHand, leftHand);

            var rightHand = ParticleObject.transform.Find("RightHand");
            if (rightHand)
                SetRigParticle(ani, HumanBodyBones.RightHand, rightHand);

            var leftFoot = ParticleObject.transform.Find("LeftFoot");
            if (leftFoot)
                SetRigParticle(ani, HumanBodyBones.LeftFoot, leftFoot);

            var rightFoot = ParticleObject.transform.Find("RightFoot");
            if (rightFoot)
                SetRigParticle(ani, HumanBodyBones.RightFoot, rightFoot);

            var world = ParticleObject.transform.Find("World");
            if (world)
                SetWorldParticle(ani, world);

            //コピー元を非アクティブ化
            ParticleObject.SetActive(false);
        }

        private void SetRigParticle(Animator animator,HumanBodyBones bones,Transform SetObj)
        {
            Transform rig = animator.GetBoneTransform(bones);
            Logger.log?.Debug($"SetParticle RightFoot {rig.name}");
            var setObj = GameObject.Instantiate(SetObj.gameObject, Vector3.zero, Quaternion.identity, rig);
            SetParticles.Add(setObj);
            //ずれるので戻す
            setObj.transform.localPosition = SetObj.transform.position;
            //スケールをコピー
            setObj.transform.localScale = VRM.transform.localScale;

            //オブジェクト自体はコピー用のオブジェクト格納なので非アクティブにしておく
            SetObj.gameObject.SetActive(false);

        }

        private void SetWorldParticle(Animator animator, Transform SetObj)
        {
            //Worldにセットする
            var setObj = GameObject.Instantiate(SetObj.gameObject, Vector3.zero, Quaternion.identity);
            SetParticles.Add(setObj);
            //ずれるので戻す
            setObj.transform.localPosition = SetObj.transform.localPosition;
            //スケールをコピー
            setObj.transform.localScale = VRM.transform.localScale;
            //デストロイされないようにする
            DontDestroyOnLoad(setObj);

            //オブジェクト自体はコピー用のオブジェクト格納なので非アクティブにしておく
            SetObj.gameObject.SetActive(false);

            Logger.log?.Debug($"SetParticle World {setObj.name}");

        }

    }
}
