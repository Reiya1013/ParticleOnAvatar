
using IPA.Config.Stores;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace ParticleOnAvatar.Parameter
{
    class PluginParameter
    {

        public static PluginParameter Instance { get; set; }
        public PluginParameter()
        {
        }

        static PluginParameter()
        {
            Instance = new PluginParameter();
        }

        public virtual float Offset_XPosition { get; set; } = 0f;
        public virtual float Offset_YPosition { get; set; } = 0f;
        public virtual float Offset_ZPosition { get; set; } = -2.0f;


        public virtual float Offset_XRotation { get; set; } = 0f;
        public virtual float Offset_YRotation { get; set; } = 0f;
        public virtual float Offset_ZRotation { get; set; } = 0f;


        public event Action<PluginParameter> OnChangedEvent;


        public Vector3 Position()
        {
            Logger.log.Debug($"{Offset_XPosition} {Offset_YPosition} {Offset_ZPosition} {new Vector3(Offset_XPosition, Offset_YPosition, Offset_ZPosition)}");
            return new Vector3(Offset_XPosition, Offset_YPosition, Offset_ZPosition);
        }

        public Quaternion Rotation()
        {
            Logger.log.Debug($"{Offset_XRotation} {Offset_YRotation} {Offset_ZRotation}");
            return Quaternion.Euler(new Vector3(Offset_XRotation, Offset_YRotation, Offset_ZRotation));
        }

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload() =>
            // Do stuff after config is read from disk.
            this.OnChangedEvent?.Invoke(this);

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed() =>
            // Do stuff when the config is changed.
            this.OnChangedEvent?.Invoke(this);

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginParameter other)
        {
            // This instance's members populated from other
        }
    }
}
