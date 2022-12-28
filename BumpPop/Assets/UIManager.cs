using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Signals;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ballText;
    [SerializeField] private TextMeshProUGUI moneyText;


    private void Start()
    {
        DOTween.Init();
        var text = "" + GameManager.Instance.BallCount;
        ballText.DOText(text, 0.1f).SetEase(Ease.InOutQuad);
    }

    void ChangeBallText(int count)
    {
        var text = "" + count;
        ballText.DOText(text, 0.1f).SetEase(Ease.InOutQuad);
    }

    void ChangeMoneyText(int count)
    {
        var text = "" + count * 7;
        moneyText.DOText(text, 0.1f).SetEase(Ease.InOutQuad);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnCloneBall += ChangeBallText;
        CoreGameSignals.Instance.OnCloneBall += ChangeMoneyText;
    }

    void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.OnCloneBall -= ChangeBallText;
        CoreGameSignals.Instance.OnCloneBall -= ChangeMoneyText;
    }
}