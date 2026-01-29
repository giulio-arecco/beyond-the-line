using Storage.StorableInfo;
using UnityEngine;

namespace Storage.StorableInfoDatabase {
    [CreateAssetMenu(fileName = "CompanionInfoDatabase", menuName = "Scriptable Objects/Storage/StorableInfoDatabase/CompanionInfoDatabase", order = 1)]
    public class CompanionInfoDatabaseSO : StorableInfoDatabaseSO<CompanionInfoSO> {}
}