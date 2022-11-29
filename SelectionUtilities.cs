using Mastercam.IO;
using Mastercam.Database.Types;
using Mastercam.GeometryUtility;


namespace eMastercamRateMyCode
{
    internal class SelectionUtilities
    {
        public void UnselectAll() 
            => SelectionManager.UnselectAllGeometry();

        public void TurnOffVisibleLevels(bool refreshLevelsList = true)
        {
            var shown = LevelsManager.GetVisibleLevelNumbers();
            foreach (var level in shown)
            {
                LevelsManager.SetLevelVisible(level, false);
            }
            if (refreshLevelsList)
                LevelsManager.RefreshLevelsManager();
        }

        public void SetMainLevel(int levelNumber, bool makeVisible = true, bool refreshLevelsList = true)
        {
            LevelsManager.SetMainLevel(levelNumber);
            LevelsManager.SetLevelVisible(levelNumber, makeVisible);

            if (refreshLevelsList)
                LevelsManager.RefreshLevelsManager();

            GraphicsManager.Repaint(true);
        }

        public bool CreateLevel(int levelNumber, string levelName, bool setAsMain = true, bool makeVisible = true)
        {
            var result = LevelsManager.SetLevelName(levelNumber, levelName);

            if (setAsMain)
                LevelsManager.SetMainLevel(levelNumber);

            LevelsManager.SetLevelVisible(levelNumber, makeVisible);

            LevelsManager.RefreshLevelsManager();

            return result;
        }

        public int MoveSelectedToLevel(int levelNumber, bool clearSelection = true, bool clearColors = true)
        {
            var numberOfEntsCopied = GeometryManipulationManager.MoveSelectedGeometryToLevel(levelNumber, clearSelection);

            if (clearColors)
                GraphicsManager.ClearColors(new GroupSelectionMask(true));

            return numberOfEntsCopied;
        }   
        
        public void ClearGroupColors()
        {
            GraphicsManager.ClearColors(new GroupSelectionMask(true));
            GraphicsManager.Repaint(true);
        }
    }
}
