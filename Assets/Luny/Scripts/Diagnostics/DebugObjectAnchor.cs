using UnityEngine;

public sealed class DebugObjectAnchor : MonoBehaviour
{
	[Header("Target")]
	[SerializeField] private Transform _target;
	[SerializeField] private Vector3 _offset;

	private Camera _mainCam;
	private RectTransform _rectTransform;

	private void Start()
	{
		_mainCam = Camera.main;
		_rectTransform = transform as RectTransform;
	}

	private void LateUpdate()
	{
		if (_target == null || _mainCam == null)
			return;

		var screenPos = _mainCam.WorldToScreenPoint(_target.position + _offset);
		if (screenPos.z >= 0)
		{
			_rectTransform.localScale = Vector3.one;
			transform.position = screenPos;
		}
		else
			_rectTransform.localScale = Vector3.zero;
	}
}
