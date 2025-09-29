using Storage.StorableInfo;
using UnityEngine;

namespace Storage.StorableInfoDatabase {
    [CreateAssetMenu(fileName = "CompanionInfoDatabase", menuName = "Scriptable Objects/StorableInfoDatabase/CompanionInfoDatabase")]
    public class CompanionInfoDatabaseSO : StorableInfoDatabaseSO<CompanionInfoSO> {}
}