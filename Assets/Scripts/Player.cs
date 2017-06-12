using UnityEngine;
using System;

namespace Bomberman
{
    public class Player : MovingObject, IDestroyable
    {
        public int bombPower;
        public GameObject bomb;

        public event EventHandler<BombChangedArgs> OnBombChanged;

        [SerializeField]
        private int _maxBombs;
        public int MaxBombs {
            get { return _maxBombs; }
            set {
                _maxBombs = value;
                OnBombChanged(this, new BombChangedArgs(_maxBombs - Bombs));
            }
        }

        private int _bombs;
        private int Bombs {
            get { return _bombs; }
            set {
                _bombs = value;
                if (_bombs > MaxBombs) {
                    _bombs = MaxBombs;
                }
                if (OnBombChanged != null) {
                    OnBombChanged(this, new BombChangedArgs(MaxBombs - _bombs));
                }
            }
        }

        protected override void Start() {
            base.Start();
            GameManager.instance.SetPlayer(this);
        }

        private void Update() {
            Move();
            Attack();
        }

        private void Attack() {
            if (Input.GetKeyUp(KeyCode.Space) && Bombs < MaxBombs) {
                //Spawn bomb in cell center
                Vector3 bombPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
                Bomb bombInstance = Instantiate(bomb, bombPosition, Quaternion.identity).GetComponent<Bomb>();
                bombInstance.power = bombPower;
                bombInstance.OnBang += delegate { Bombs--; };
                Bombs++;
            }
        }

        private void Move() {

            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            //Disable move by diagonals
            if (horizontal != 0) {
                vertical = 0;
            }

            AttemptMove(horizontal, vertical);
        }


        protected bool AttemptMove(int xDir, int yDir) {
            if (xDir == 0 && yDir == 0) {
                return true;
            }

            Vector2 start = transform.position;
            Vector2 direction = new Vector2(xDir, yDir);
            Vector2 halfSizeDirection = new Vector2(halfWidth * xDir, halfHeight * yDir);

            Vector2 end = start + direction * speed;

            UpdateAnimation(xDir, yDir);

            //Disable collider because linecast hit self
            boxCollider.enabled = false;
            //Offset from center to edge
            Vector2 offsetVector = new Vector2(halfWidth * yDir, halfHeight * xDir) * 0.75f;

            //Sends 2 rays from the left and right edges, instead of the center. If at least one has found a collider - the player can not move
            RaycastHit2D hit = Physics2D.Linecast(start + offsetVector, end + offsetVector + halfSizeDirection, blockingLayer);
            if (hit.transform == null) {
                offsetVector *= -1;
                hit = Physics2D.Linecast(start + offsetVector, end + offsetVector + halfSizeDirection, blockingLayer);
            }
            boxCollider.enabled = true;


            if (hit.transform == null) {
                rb2D.MovePosition(end);
                return true;
            }

            Vector2 position = hit.point - halfSizeDirection - offsetVector;

            //Ignoring the corners of the walls
            if (Mathf.Abs((position.x % 1) - 0.5f) >= 0.3f) {
                position.x = Mathf.RoundToInt(position.x);
            }
            if (Mathf.Abs((position.y % 1) - 0.5f) >= 0.3f) {
                position.y = Mathf.RoundToInt(position.y);
            }
            rb2D.MovePosition(position);


            return false;
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Exit") {
                rb2D.MovePosition(other.transform.position);
                GameManager.instance.Win();
                enabled = false;
            }

            else if (other.tag == "Bonus") {
                other.GetComponent<AbstractBonus>().Use(this);
            }
        }


        public void TakeDamage() {
            //Chek shield if not - DIE!
            GameManager.instance.GameOver();
        }


        public void Destroy() {
            Destroy(gameObject);
            GameManager.instance.GameOver();
        }

        void OnCollisionEnter2D(Collision2D other) {
            if (other.collider.tag == "Enemy") {
                TakeDamage();
            }
        }
    }
}