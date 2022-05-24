using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int _value = 2;
    [SerializeField] private TMP_Text text;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isAnimating;
    private float _count;
    [SerializeField] private TileSettings tileSettings;
    private Animator _animator;
    private TileManager _tileManager;
    private Image _tileImage;

    public void SetValue(int value)
    {
        _value = value;
        text.text = value.ToString();
        TileColor newColor = tileSettings.TileColors.FirstOrDefault(color => color.value == _value) ?? new TileColor();
        text.color = newColor.fgColor;
        _tileImage.color = newColor.bgColor;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _tileManager = FindObjectOfType<TileManager>();
        _tileImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (!_isAnimating)
            return;
        _count += Time.deltaTime;

        float t = _count / tileSettings.AnimationTime;
        t = tileSettings.AnimationCurve.Evaluate(t);

        Vector3 newPos = Vector3.Lerp(_startPos, _endPos, t);

        transform.position = newPos;

        if (_count >= tileSettings.AnimationTime)
        {
            _isAnimating = false;
            if (_mergeTile != null)
            {
                int newValue = _value + _mergeTile._value;
                _tileManager.AddScore(newValue);
                SetValue(newValue);
                Destroy(_mergeTile.gameObject);
                _animator.SetTrigger("Merge");
                _mergeTile = null;
            }
        }
    }

    private Tile _mergeTile;

    public bool Merge(Tile otherTile)
    {
        if (this._value != otherTile._value)
            return false;

        if (_mergeTile != null || otherTile._mergeTile != null)
            return false;

        _mergeTile = otherTile;

        return true;
    }

    public void SetPosition(Vector3 newPos, bool instant)
    {
        if (instant)
        {
            transform.position = newPos;
            return;
        }
        _startPos = transform.position;
        _endPos = newPos;
        _count = 0;
        _isAnimating = true;

        if (_mergeTile != null)
        {
            _mergeTile.SetPosition(newPos, false);
        }
    }

    public int GetValue()
    {
        return _value;
    }
}
