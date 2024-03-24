using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using RobotManager.Utility;
using RobotMananager;

namespace RobotManager.Controls.Custom;

public sealed class PlayfieldOverview : Panel {

    //GraphicsEntity Class
    //DrawRectRelative
    //Helper Stuff

    private const int WallWidth = 5;
    private const int EntitySize = 50;
    private const int GridSize = 50;
    private const int Grids = 10;

    private DataStructures.WebotsMap WeMap;
    private readonly bool InDesign = FormEx.IsInDesignMode();

    public PlayfieldOverview(DataStructures.WebotsMap WeMap) {
        this.WeMap = WeMap;
        
        AutoSize = false;
        Height = (GridSize * Grids) + (WallWidth * 2);
        Width = Height;
        MaximumSize = new(Height,Width);
        MinimumSize = new(Height,Width);

        if (InDesign || Disposing || IsDisposed)
            return;

        if (!IsHandleCreated)
            CreateHandle();

        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        Update();
    }

    protected override void OnPaint(PaintEventArgs e) {
        DrawMap_BackGround(e.Graphics, this);
        DrawMap_Border(e.Graphics, this);
        DrawMap_Entities(e.Graphics);
    }

    new public void Update() {
        Invalidate();
        Refresh();
    }

    private void DrawMap_Entities(Graphics G) {
        for (int Y = 0; Y < Grids; Y++) {

            for (int X = 0; X < Grids; X++) {
                DrawEntity_Wall(G, X, Y);
                DrawEntity_Spawn(G, X, Y);
                DrawEntity_Robot(G, X, Y);
            }

            for (int X = 0; X < Grids; X++) {
                //Flag Entity
                DrawDynamicEntity(G, X, Y);
            }

        }
    }

    private void DrawEntity_Robot(Graphics G, int X, int Y) {
        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Robot1)
            _ = new GraphicsEntity(G, X, Y, Color.Yellow);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Robot2)
            _ = new GraphicsEntity(G, X, Y, Color.Red);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Robot3)
            _ = new GraphicsEntity(G, X, Y, Color.Green);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Robot4)
            _ = new GraphicsEntity(G, X, Y, Color.Blue);
    }



    private void DrawDynamicEntity(Graphics G, int X, int Y) {

        //Flag Rendering,
        //A flag is assigned to a robot,
        //need to find out which robot,
        //If have robot set the flag to this color,
        //if flag has no robot set it to black
    }

    //--------------------------
    //  Static Entity Draw
    //--------------------------

    private void DrawEntity_Wall(Graphics G, int X, int Y) {
        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Wall)
            _ = new GraphicsEntity(G, X, Y, Color.Brown);
    }

    private void DrawEntity_Spawn(Graphics G, int X, int Y) {
        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Spawn1)
            _ = new GraphicsEntity(G, X, Y, Color.DarkGoldenrod);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Spawn2)
            _ = new GraphicsEntity(G, X, Y, Color.DarkRed);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Spawn3)
            _ = new GraphicsEntity(G, X, Y, Color.DarkGreen);

        if (WeMap.GetTile(X, Y) == DataStructures.WeTile.Spawn4)
            _ = new GraphicsEntity(G, X, Y, Color.DarkBlue);
    }

    //--------------------------
    //  Background Draw
    //--------------------------

    private static void DrawMap_BackGround(Graphics G, Control Pnl) {
        Brush Brsh = new SolidBrush(Color.Beige);

        Point[] BG = [
            new (0, 0), //Topleft
            new (Pnl.Width, 0), //TopRight
            new (Pnl.Width, Pnl.Height),
            new (0, Pnl.Height)
        ];

        G.FillPolygon(Brsh, BG);
    }

    private static void DrawMap_Border(Graphics G, Control Pnl) {

        Brush Brsh = new SolidBrush(Color.Black);

        Point[] WallL = [
            new (0, 0), //Topleft
            new (WallWidth, 0), //TopRight
            new (WallWidth, Pnl.Height),  //BottomRight
            new (0, Pnl.Height) // BottomLeft
        ];

        Point[] WallR = [
            new (Pnl.Width, 0),
            new (Pnl.Width - WallWidth, 0),
            new (Pnl.Width - WallWidth, Pnl.Height),
            new (Pnl.Width, Pnl.Height)
        ];

        Point[] WallT = [
            new (0, 0),
            new (Pnl.Width, 0),
            new (Pnl.Width, WallWidth),
            new (0, WallWidth)
        ];

        Point[] WallB = [
            new (0, Pnl.Height - WallWidth),
            new (Pnl.Width, Pnl.Height - WallWidth),
            new (Pnl.Width, Pnl.Height),
            new (0, Pnl.Height)
        ];

        G.FillPolygon(Brsh, WallL);
        G.FillPolygon(Brsh, WallR);
        G.FillPolygon(Brsh, WallT);
        G.FillPolygon(Brsh, WallB);

    }

    private class GraphicsEntity {

        public GraphicsEntity(Graphics G, int GridX, int GridY , Color C) {

            Brush EntBrush = new SolidBrush(C);
            Point[] Obj = [
                new ((WallWidth + GridY * GridSize), (WallWidth + GridX * GridSize)), //Topleft
                new ((WallWidth + GridY * GridSize) + (EntitySize - WallWidth / Grids) , (WallWidth + GridX * GridSize)), //TopRight
                new ((WallWidth + GridY * GridSize) + (EntitySize - WallWidth / Grids), (WallWidth + GridX * GridSize) + (EntitySize - WallWidth / Grids)), //BottomRight
                new ((WallWidth + GridY * GridSize), (WallWidth + GridX * GridSize) + (EntitySize - WallWidth / Grids)) //BottomLeft
            ];

            G.FillPolygon(EntBrush, Obj);
        }

    }

    private class GraphicsEntityFlag {

        public GraphicsEntityFlag(IDeviceContext G, int GridX, int GridY, Color C) {
            TextRenderer.DrawText(G, "FL", new Font("Consolas", 25, FontStyle.Regular), new Point((WallWidth + GridX * GridSize) - 2, (WallWidth + GridY * GridSize) + 5), C);
        }

    }





}
