
public class CardViewPool : PoolManager<CardBattleView>
{
    protected override void OnCreateElement(CardBattleView element)
    {
        element.transform.position = transform.position;
        element.gameObject.SetActive(false);
    }

    protected override void OnPullElement(CardBattleView element)
    {
        element.gameObject.SetActive(true);
    }

    protected override void OnRecycleElement(CardBattleView element)
    {
        element.transform.SetParent(transform);
        element.transform.position = transform.position;
        element.gameObject.SetActive(false);
    }
}
