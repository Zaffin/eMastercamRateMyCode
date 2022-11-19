using Mastercam.App;
using Mastercam.App.Types;


namespace eMastercamRateMyCode
{
    public class Main : NetHook3App
    {
        public override MCamReturn Run(int parameter)
        {
            var utilities = new UtilitiesFacade();

            var lowerCrease = new EntityData()
            {
                ColorID = 11,
                SmallOffsetRadius = 0.040,
                LargeOffsetRadius = 0.065,
                Level = new LevelData(501, "Lower Created Crease Geo")
            };

            var upperCrease = new EntityData()
            {
                ColorID = 10,
                SmallOffsetRadius = 0.014,
                LargeOffsetRadius = 0.014,
                Level = new LevelData(500, "Upper Created Crease Geo")
            };

            var result = utilities.OffsetCreaseChain(101, lowerCrease, upperCrease);
            if (result.wasGeometryCreated)
            {
                var lowerData = new EntityData()
                {
                    ColorID = 10,
                    Level = new LevelData(503, "Lower Connected Lines")
                };
                utilities.ConnectLines(lowerData, result.lowerLines);

                var upperData = new EntityData()
                {
                    ColorID = 11,
                    Level = new LevelData(502, "Upper Connected Lines")
                };
                utilities.ConnectLines(upperData, result.upperLines);
            }

            var lowerCut = new EntityData()
            {
                ColorID = 11,
                SmallOffsetRadius = 0.0025,
                LargeOffsetRadius = 0.0225,
                Level = new LevelData(501, "Lower Created Cut Geo")
            };

            var upperCut = new EntityData()
            {
                ColorID = 10,
                SmallOffsetRadius = 0.0025,
                LargeOffsetRadius = 0.0385,
                Level = new LevelData(500, "Upper Created Cut Geo")
            };

            utilities.OffsetCutChain(75, lowerCut, upperCut);


            return MCamReturn.NoErrors;
        }

    }
}
