using UnityEngine;

public class BackGround : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    float _offset;
    float _speed = 0.05f;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _offset = Time.deltaTime * _speed;

        if(_offset > 1.0f)
        {
            _offset -= 1.0f;
        }
        _spriteRenderer.material.mainTextureOffset += new Vector2(_offset, 0);
    }
}