//Native Imports

using System;
using System.Windows.Forms;

//Library Imports
using BlueMystic;
using RobotManager.Controls.Custom;

//Local Imports
using RobotManager.Utility;
using RobotManager.Utility.Debug;
using RobotMananager;
using static RobotManager.Utility.FormEx;

namespace RobotManager.Forms;
public partial class FormMain : Form {

    //Singleton Instance
    public static FormMain Instance;

    //---------------------
    //     Vars
    //---------------------

    //Fields
    public static DataStructures.WebotsMap Map = new();
    private PlayfieldOverview Playfield;
    //Consts
    //Events

    //Properties


    //---------------------
    //     Construtor
    //---------------------

    public FormMain() {
        if (IsInDesignMode()) {
            InitializeComponent();
            return;
        }
        PreInit();
        InitializeComponent();

        Logger.Info(".ctor &1OK");
    }

    
    //-------------------------
    //      Init
    //-------------------------

    private void PreInit() {
        Logger.Info("Pre-Init");
        //Set Singleton
        if (Instance is not null)
            throw new("Singleton Reinstantiation in FormMain");
        Instance = this;

        
        //Set All Controls to double bufferd
        GetAllChildControls(this).ForEach(X => {
            try {
                SetDoubleBuffered(X,true);
            }
            catch {
                Logger.Warn($"&3Can't Set {X.GetType()} To Double Buffered");
            }
        });

    }

    private void PostInit() {
        new DarkModeCS(this);
        Init_AddMap();
        Logger.Info("Post-Init");
    }

    private void Init_AddMap() {
        if (Playfield is not null)
            return;
        Playfield = new(Map);

        //TODO GIVE THIS A MAP OBJECT
        Playfield.Dock = DockStyle.Left;
        Playfield.Location = new(3, 3);
        Playfield.MaximumSize = new(510, 510);
        Playfield.MinimumSize = new(510, 510);
        Playfield.Name = "Playfield";
        Playfield.Size = new(510, 510);
        Playfield.TabIndex = 0;

        TabManage.Controls.Add(Playfield);
        Logger.Info("Added WebotsMap");
    }

    //-------------------------
    //      Events
    //-------------------------

    private void OnLoad(object sender, EventArgs e) {
        PostInit();
    }

    //-------------------------
    //      Buttons
    //-------------------------

}