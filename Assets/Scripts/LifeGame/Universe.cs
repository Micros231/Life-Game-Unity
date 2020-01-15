using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LifeGame
{
    /// <summary>
    /// Universe of the game "Life"
    /// </summary>
    public class Universe : MonoBehaviour
    {
        #region Public Const
        public const uint MAX_LENGTH_UNIVERSE = 100;
        public const uint MIN_LENGTH_UNIVERSE = 10;
        #endregion

        #region Public Properties
        /// <summary>
        /// Number of Rows
        /// </summary>
        public int Rows 
        { 
            get => _rows; 
            set
            {
                if (value < MIN_LENGTH_UNIVERSE || value > MAX_LENGTH_UNIVERSE)
                {
                    throw new ArgumentOutOfRangeException("Rows",
                        $"The number of rows can be less than {MIN_LENGTH_UNIVERSE} or greater than {MAX_LENGTH_UNIVERSE}");
                }
                else
                {
                    _rows = value;
                }
                    
            }
        }
        /// <summary>
        /// Number of Columns
        /// </summary>
        public int Columns
        {
            get => _columns;
            set
            {
                if (value < MIN_LENGTH_UNIVERSE || value > MAX_LENGTH_UNIVERSE)
                {
                    throw new ArgumentOutOfRangeException("Columns",
                        $"The number of columns can be less than {MIN_LENGTH_UNIVERSE} or greater than {MAX_LENGTH_UNIVERSE}");
                }
                else
                {
                    _columns = value;
                }
            }
        }
        /// <summary>
        /// Number of Generations
        /// </summary>
        public int Generation { get; private set; }
        /// <summary>
        /// Chance of revitalizing a cell
        /// </summary>
        public float ChanceRevializingCell
        {
            get => _chanceRevializingCell;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("ChanceRevializingCell", 
                        "Chance revitalizing cells may not be less than 0 or greater than 1");
                }
                else
                {
                    _chanceRevializingCell = value;
                }
                    
            }
        }
        /// <summary>
        /// Timer until the next generation
        /// </summary>
        public float NextGenerationTimer
        {
            get => _nextGenerationTimer;
            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("NextGenerationTimer", 
                        "The timer cannot be less than 0");
                }
                else
                {
                    _nextGenerationTimer = value;
                }
            } 
        }
        #endregion

        #region Public Fields
        [HideInInspector]
        public UnityEvent OnNextGeneration;
        #endregion

        #region Private SerializeFields
        [SerializeField]
        private Camera _camera;
        [SerializeField] 
        private GameObject _cellPrefab;
        [SerializeField, Range(MIN_LENGTH_UNIVERSE,MAX_LENGTH_UNIVERSE)]
        private int _rows = 10;
        [SerializeField, Range(MIN_LENGTH_UNIVERSE, MAX_LENGTH_UNIVERSE)]
        private int _columns = 10;
        [SerializeField, Range(0, 1)]
        private float _chanceRevializingCell = 0.3f;
        [SerializeField]
        private float _nextGenerationTimer = 1;
        [SerializeField, HideInInspector]
        private bool _isRunGeneration;
        #endregion

        #region Private Fields
        private Cell[,] _cellMatrix;
        private Transform _gridTransform;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            InitUniverse();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialization game and start generation
        /// </summary>
        public void StartGeneration()
        {
            InitGame();
            InvokeRepeating(nameof(NextGeneration), 0, _nextGenerationTimer);
            _isRunGeneration = true;
        }
        /// <summary>
        /// Resume Generation
        /// </summary>
        public void ResumeGeneration()
        {
            InvokeRepeating(nameof(NextGeneration), 0, _nextGenerationTimer);
            _isRunGeneration = true;
        }
        /// <summary>
        /// Pause Generation
        /// </summary>
        public void PauseGeneration()
        {
            CancelInvoke(nameof(NextGeneration));
            _isRunGeneration = false;
        }
        /// <summary>
        /// Stop Generation
        /// </summary>
        public void StopGeneration()
        {
            CancelInvoke(nameof(NextGeneration));

            Generation = 0;

            Destroy(_gridTransform.gameObject);
            _isRunGeneration = false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Component Initialization
        /// </summary>
        private void InitUniverse()
        {
            if (_camera == null)
            {
                _camera = Camera.current;
            }
        }
        /// <summary>
        /// Game initialization
        /// </summary>
        private void InitGame()
        {
            Generation = 0;
            _cellMatrix = new Cell[_columns, _rows];
            _gridTransform = new GameObject("Grid").GetComponent<Transform>();

            _camera.transform.position = new Vector3(_columns / 2, _rows / 2, -10f);
            _camera.GetComponent<Camera>().orthographicSize = (_columns > _rows ? (_columns / 2 + 1) : (_rows / 2 + 1));

            Vector2 cellPosition = Vector2.zero;
            Cell cell;
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    cellPosition.x = x;
                    cellPosition.y = y;

                    GameObject cellInstance = Instantiate(_cellPrefab, cellPosition, Quaternion.identity);
                    cellInstance.transform.SetParent(_gridTransform);

                    cell = cellInstance.GetComponent<Cell>();

                    cell.isAlive = (UnityEngine.Random.value <= _chanceRevializingCell ? true : false);

                    _cellMatrix[x, y] = cell;
                }
            }
        }
        /// <summary>
        /// Miscalculation of all cells and setting them new states
        /// </summary>
        private void NextGeneration()
        {
            for (int i = 0; i < _cellMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _cellMatrix.GetLength(1); j++)
                {
                    _cellMatrix[i, j].ScanNeighbors(i, j, _cellMatrix);
                }
            }
            for (int i = 0; i < _cellMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _cellMatrix.GetLength(1); j++)
                {
                    _cellMatrix[i, j].Determine();
                }
            }
            Generation++;
            OnNextGeneration.Invoke();
        }
       
        #endregion

    }
}


