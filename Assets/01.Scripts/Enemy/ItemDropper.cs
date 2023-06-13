using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private ItemDropTableSO _dropTable;
    private float[] _itemWeights;

    [SerializeField]
    [Range(0, 1f)]
    private float _dropChance;

    private void Start()
    {
        _itemWeights = _dropTable.DropList.Select(item => item.Rate).ToArray();
    }

    public void DropItem()
    {
        float ratio = Random.value; // 0 ~ 1까지의 값이 나와

        if(ratio < _dropChance)  //이러면 드랍
        {
            int idx = GetRandomWeightedIndex();
            PoolableMono resource = PoolManager.Instance.Pop(
                                _dropTable.DropList[idx].ItemPrefab.name);
            resource.transform.position = transform.position;
        }
    }

    private int GetRandomWeightedIndex()
    {
        float sum = 0f;
        for (int i = 0; i < _itemWeights.Length; i++)
        {
            sum += _itemWeights[i];
        }

        float randomValue = Random.Range(0f, sum);
        float tempSum = 0;

        for(int i = 0; i < _itemWeights.Length; i++)
        {
            if(randomValue >= tempSum && randomValue < tempSum + _itemWeights[i])
            {
                return i;
            }else
            {
                tempSum += _itemWeights[i];
            }
        }
        
        return 0;
    }
}
