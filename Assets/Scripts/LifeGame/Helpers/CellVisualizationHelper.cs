using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeGame
{

    /// <summary>
    /// Cell Visualization Helper
    /// </summary>
    [Serializable]
    public class CellVisualizationHelper
    {
        /// <summary>
        /// SpriteRenderer Component
        /// </summary>
        public SpriteRenderer SpriteRenderer { get; }

        [SerializeField] 
        private Color AliveColor = Color.white;
        [SerializeField] 
        private Color DeadColor = Color.black;
        [SerializeField] 
        private Sprite _whiteSprite;

        /// <summary>
        /// Initialization Helper 
        /// </summary>
        /// <param name="spriteRenderer">SpriteRenderer Component</param>
        /// <param name="oldVisualizationHelper">Old CellVisualizationHelper</param>
        public CellVisualizationHelper(SpriteRenderer spriteRenderer, CellVisualizationHelper oldVisualizationHelper)
        {
            SpriteRenderer = spriteRenderer;
            _whiteSprite = oldVisualizationHelper._whiteSprite;
            AliveColor = oldVisualizationHelper.AliveColor;
            DeadColor = oldVisualizationHelper.DeadColor;
            SpriteRenderer.sprite = _whiteSprite;   
        }
        /// <summary>
        /// Cell state visualization
        /// </summary>
        /// <param name="isAlive">State of the cell</param>
        public void SetStateVisualization(bool isAlive)
        {
            SpriteRenderer.color = isAlive ? AliveColor : DeadColor;
        }
    }
}

