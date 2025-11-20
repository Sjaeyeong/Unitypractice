using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].Onclick();
    }

    void Next()
    {
        // 1. all item disable
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. random 3 items enable
        int[] random = new int[3];
        while (true)
        {
            random[0] = Random.Range(0, items.Length);
            random[1] = Random.Range(0, items.Length);
            random[2] = Random.Range(0, items.Length);

            if(random[0]!=random[1] && random[0]!=random[2] && random[1]!=random[2])
                break;
        }

        for (int i = 0; i < random.Length; i++)
        {
            Item ranItem = items[random[i]];

            // 3. maxlevel - replace usableitem.
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true); // 소비 아이템이 여러개면 items[Random.Range(소비아이템 인덱스 범위)]
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
