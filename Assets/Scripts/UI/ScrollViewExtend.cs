using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;


public delegate void evnetHandler(int value);

public class ScrollViewExtend : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    public int totalPageNum; // 总页数
    private int _currentPage; // 当前页数
    private bool _isDrag; // 是否在拖拽状态
    private bool _isMoving; // 是否在移动状态
    private float[] _pagePos; // 每一页在水平方向的位置
    private ScrollRect _scrollRect;
    public event evnetHandler PageChange;

    public Text pageText;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _pagePos = new float[totalPageNum];
        for(int i=0;i<totalPageNum;i++)
        {
            _pagePos[i] = (float)i / (totalPageNum - 1);
        }
        Init();
    }

    public void Init()
    {
        _isDrag = false;
        _isMoving = false;
        _currentPage = 0;
        if (pageText != null)
        {
            pageText.text = (_currentPage + 1).ToString() + "/" + totalPageNum;
        }
        _scrollRect.horizontalNormalizedPosition = 0;
    }

    public void ChangeTotalNum(int num)
    {
        totalPageNum = num;
        _pagePos = new float[totalPageNum];
        for (int i = 0; i < totalPageNum; i++)
        {
            _pagePos[i] = (float)i / (totalPageNum - 1);
        }
        Init();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDrag) return;
        // 获取当前位置
        float currentPos = _scrollRect.horizontalNormalizedPosition;
        // 获取当前页数
        int pageNum = GetCurrentPageNum(currentPos);
        // 移动到目标页数
        MoveToPage(pageNum);
        // 更新文本
        _currentPage = pageNum;
        if (pageText != null)
            pageText.text = (_currentPage + 1).ToString() + "/" + totalPageNum;
        GameManager.instance.audioManager.PlayPagingAudioClip();
        _isDrag = false;
    }

    private void MoveToPage(int pageNum)
    {
        if (_isMoving) return;
        _isMoving = true;
        if (PageChange != null)
        {
            PageChange(pageNum);
        }
        DOTween.To(() => _scrollRect.horizontalNormalizedPosition,
            lerpValue => _scrollRect.horizontalNormalizedPosition = lerpValue,
            _pagePos[pageNum], 0.5f)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => _isMoving = false);
    }

    public int GetCurrentPageNum(float pos)
    {
        int pageNum = 0;
        float offset = 1;
        for (int i=0;i<totalPageNum;i++)
        {
            float _offset = Mathf.Abs(_pagePos[i] - pos);
            if (_offset < offset)
            {
                pageNum = i;
                offset = _offset;
            }
        }
        return pageNum;
    }

    public void ToNextPage()
    {
        if (_isMoving) return;
        if (_currentPage >= (totalPageNum - 1)) return;
        MoveToPage(++_currentPage);
        if (pageText != null)
            pageText.text = (_currentPage + 1).ToString() + "/" + totalPageNum;
        GameManager.instance.audioManager.PlayPagingAudioClip();
    }

    public void ToLastPage()
    {
        if (_isMoving) return;
        if (_currentPage <= 0) return;
        MoveToPage(--_currentPage);
        if (pageText != null)
            pageText.text = (_currentPage + 1).ToString() + "/" + totalPageNum;
        GameManager.instance.audioManager.PlayPagingAudioClip();
    }
}
