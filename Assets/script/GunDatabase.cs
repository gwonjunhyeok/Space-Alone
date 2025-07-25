using System.Collections.Generic;

namespace GunsData
{
    public static class GunDatabase
    {
        public static readonly Dictionary<string, GunData> GunDict = new Dictionary<string, GunData>
    {
        {
            "basic_gun",
            new GunData
            {
                gunName = "basic_gun",
                maxMagazine = 30,
                fireDelay = 0.1f
            }
        },
        {
            "shotgun",
            new GunData
            {
                gunName = "shotgun",
                maxMagazine = 8,
                fireDelay = 0.8f
            }
        }
        // 총기를 계속 여기서 추가
    };
    }
}
