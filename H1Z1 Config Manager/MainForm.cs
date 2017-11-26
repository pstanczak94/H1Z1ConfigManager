using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Globalization;
using System.Threading;

namespace H1Z1_Config_Manager
{
    public partial class MainForm : Form
    {
        public UserOptionsParser userOptions;
        public ConsoleForm conForm;
        public SettingsForm setForm;
        public AboutForm aboutForm;
        public string pathFromRegistry;

        public MainForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            userOptions = null;
            conForm = null;
            setForm = null;
            aboutForm = null;

            pathFromRegistry = String.Empty;

            MinimumSize = Size;
            MaximumSize = MinimumSize;

            //MaximumSize = new Size((int)(Width * 2.0), (int)(Height * 1.5));

            KeyDown += MainForm_KeyDown;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveConfigPrompt();
                e.SuppressKeyPress = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = Application.ProductName + " " + Application.ProductVersion;

            SettingsForm.CheckPathIsInRegistry(out pathFromRegistry);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            InitDefaultSwitchButtonStates();
            Settings.Init();
            Settings.Read();
            LoadUserOptions();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save();
        }

        public bool LoadUserOptions()
        {
            string gamePath = Settings.UserOptionsPath;

            if (string.IsNullOrWhiteSpace(gamePath))
            {
                if (string.IsNullOrWhiteSpace(pathFromRegistry))
                {
                    Logger.WriteLine(LogType.ERROR, "Cannot load UserOptions file! Path not found!");
                    MessageBox.Show(
                        "Cannot find UserOptions file.\r\nGo to Edit->Settings and set valid path.",
                        "UserOptions load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                gamePath = pathFromRegistry;
                Settings.UserOptionsPath = pathFromRegistry;
            }
            
            if (!SettingsForm.IsUserOptionsPathValid(ref gamePath))
            {
                Logger.WriteLine(LogType.ERROR, "Cannot load UserOptions file! Path invalid!");
                MessageBox.Show(
                        "UserOptions file path is invalid.\r\nGo to Edit->Settings and set valid path.",
                        "UserOptions load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            userOptions = new UserOptionsParser(gamePath);

            if (userOptions.ReadConfig())
            {
                InitUserOptionsValues(userOptions);
                return true;
            }

            userOptions = null;

            MessageBox.Show(
                        "UserOptions file cannot be read...",
                        "UserOptions load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        public void InitDefaultSwitchButtonStates()
        {
            btnStartingMode.InitStates("Yes:1|No:2", "Yes");
            btnFullscreenMode.InitStates("Fullscreen:1|Windowed:2", "Fullscreen");
            btnOverall.InitStates("Custom:-1|Very low:0|Low:1|Medium:2|High:3|Ultra:4", "Custom");
            btnTextures.InitStates("Low:1|Medium:2|High:3|Ultra:4", "Low");
            btnEffects.InitStates("Low:1|Medium:2|High:3", "Low");
            btnModels.InitStates("Low:1|Medium:2|High:3", "Low");
            btnFlora.InitStates("Low:1|Medium:2|High:3", "Low");
            btnTreeQuality.InitStates("Low:0|Medium:1|High:2|Ultra:3", "Low");
            btnParticle.InitStates("Low:0|Medium:1|High:2|Ultra:3", "Low");
            btnShadows.InitStates("Off:0|Low:1|Medium:2|High:3|Ultra:4", "Low");
            btnFogShadows.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnLightning.InitStates("Low:0|High:1", "Low");
            btnInteriorLightning.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnUseLod0a.InitStates("No:0|Yes:1", "No");
            btnAmbientOcclusion.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnVSync.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnSmoothing.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnDepthOfField.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnMotionBlur.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnMRawInput.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnMReduceInputLag.InitStates("Disabled:0|Enabled:1", "Disabled");
            btnMSmoothing.InitStates("Disabled:0|Enabled:1", "Disabled");

            chkMuteAll.SetStates("1", "0");
            chkShowCOF.SetStates("1", "0");
            chkClassicReticle.SetStates("1", "0");
            chkLegacyHitmarker.SetStates("1", "0");
            chkCenterInventory.SetStates("1", "0");
        }

        public void InitUserOptionsValues(UserOptionsParser parser)
        {
            readOnlyToolStripMenuItem.Checked = userOptions.ReadOnly;

            string displayMode = parser.GetStringValue("Display", "Mode");
            string displayFullscreenMode = parser.GetStringValue("Display", "FullscreenMode");

            if (displayMode.Equals("Fullscreen") || displayMode.Equals("WindowedFullscreen"))
                btnStartingMode.ChangeStateByValue(2);
            else if (displayMode.Equals("Windowed"))
                btnStartingMode.ChangeStateByValue(1);

            if (displayFullscreenMode.Equals("Fullscreen"))
                btnFullscreenMode.ChangeStateByKey("Fullscreen");
            else if (displayFullscreenMode.Equals("Windowed"))
                btnFullscreenMode.ChangeStateByKey("Windowed");

            txtResFullWidth.SetValue(parser.GetShortValue("Display", "FullscreenWidth", 1920));
            txtResFullHeight.SetValue(parser.GetShortValue("Display", "FullscreenHeight", 1080));
            txtResWindWidth.SetValue(parser.GetShortValue("Display", "WindowedWidth", 1280));
            txtResWindHeight.SetValue(parser.GetShortValue("Display", "WindowedHeight", 720));
            txtRefreshRate.SetValue(parser.GetShortValue("Display", "RefreshRate", 0));

            btnOverall.ChangeStateByValue(parser.GetShortValue("Rendering", "OverallQuality", -1));
            btnTextures.ChangeStateByValue(parser.GetShortValue("Rendering", "TextureQuality", 3));
            btnEffects.ChangeStateByValue(parser.GetShortValue("Rendering", "EffectsQuality", 2));
            btnModels.ChangeStateByValue(parser.GetShortValue("Rendering", "ModelQuality", 1));
            btnFlora.ChangeStateByValue(parser.GetShortValue("Rendering", "FloraQuality", 1));
            btnTreeQuality.ChangeStateByValue(parser.GetShortValue("Rendering", "SpeedTreeLOD", 0));
            btnParticle.ChangeStateByValue(parser.GetShortValue("Rendering", "ParticleLOD", 2));
            btnShadows.ChangeStateByValue(parser.GetShortValue("Rendering", "ShadowQuality", 0));
            btnFogShadows.ChangeStateByValue(parser.GetShortValue("Rendering", "FogShadowsEnable", 0));
            btnLightning.ChangeStateByValue(parser.GetShortValue("Rendering", "LightingQuality", 1));
            btnInteriorLightning.ChangeStateByValue(parser.GetShortValue("Rendering", "InteriorLighting", 0));
            btnUseLod0a.ChangeStateByValue(parser.GetShortValue("Rendering", "UseLod0a", 1));
            btnAmbientOcclusion.ChangeStateByValue(parser.GetShortValue("Rendering", "AO", 0));
            btnVSync.ChangeStateByValue(parser.GetShortValue("Rendering", "VSync", 0));
            btnSmoothing.ChangeStateByValue(parser.GetShortValue("Rendering", "Smoothing", 0));
            btnDepthOfField.ChangeStateByValue(parser.GetShortValue("Rendering", "UseDepthOfField", 0));
            btnMotionBlur.ChangeStateByValue(parser.GetShortValue("Rendering", "MotionBlur", 0));
            
            txtMaxFPS.SetValue(parser.GetShortValue("Rendering", "MaximumFPS", 200));
            txtRenderDist.SetValue(parser.GetShortValue("Rendering", "RenderDistance", 1500));
            txtFOV.SetValue(parser.GetShortValue("Rendering", "VerticalFOV", 70));

            txtGamma.SetPercentValue(parser.GetDoubleValue("Rendering", "Gamma", 0.0));
            txtRenderQuality.SetPercentValue(parser.GetDoubleValue("Display", "HDPixelPlus", 1.0));

            txtMSensNormal.SetValue(parser.GetDoubleValue("General", "MouseSensitivity", 0.5));
            txtMSensADS.SetValue(parser.GetDoubleValue("General", "ADSMouseSensitivity", 0.5));
            txtMSensScoped.SetValue(parser.GetDoubleValue("General", "ScopedMouseSensitivity", 0.5));
            txtMSensVehicle.SetValue(parser.GetDoubleValue("General", "VehicleMouseSensitivity", 0.5));

            btnMRawInput.ChangeStateByValue(parser.GetShortValue("General", "MouseRawInput", 1));
            btnMReduceInputLag.ChangeStateByValue(parser.GetShortValue("General", "ReduceInputLag", 0));
            btnMSmoothing.ChangeStateByValue(parser.GetShortValue("General", "MouseSmoothing", 0));

            chkMuteAll.ChangeStateByValue(parser.GetStringValue("Sound", "MuteAll", "0"));

            txtSoundMaster.SetValue(parser.GetDoubleValue("Sound", "Master", 1.0));
            txtSoundEffects.SetValue(parser.GetDoubleValue("Sound", "SoundEffects", 1.0));
            txtSoundUI.SetValue(parser.GetDoubleValue("Sound", "UI", 1.0));
            txtSoundDialog.SetValue(parser.GetDoubleValue("Sound", "Dialog", 1.0));
            txtMusicMaster.SetValue(parser.GetDoubleValue("Sound", "MusicMaster", 1.0));
            txtMusicAmbient.SetValue(parser.GetDoubleValue("Sound", "MusicAmbient", 1.0));
            txtMusicEncounter.SetValue(parser.GetDoubleValue("Sound", "MusicEncounter", 1.0));

            chkShowCOF.ChangeStateByValue(parser.GetStringValue("UI", "reticleShowCOF", "0"));
            chkClassicReticle.ChangeStateByValue(parser.GetStringValue("UI", "reticleClassicMode", "0"));
            chkLegacyHitmarker.ChangeStateByValue(parser.GetStringValue("UI", "LegacyHitmarker", "0"));
            chkCenterInventory.ChangeStateByValue(parser.GetStringValue("UI", "CenterInventory", "0"));

            if (txtResFullWidth.GetValue() == txtResWindWidth.GetValue())
                if (txtResFullHeight.GetValue() == txtResWindHeight.GetValue())
                    chkWindSameAsFull.Checked = true;

            if (txtMSensNormal.GetValue() == txtMSensADS.GetValue())
                if (txtMSensADS.GetValue() == txtMSensScoped.GetValue())
                    chkOneSensForAll.Checked = true;
        }

        public void SaveConfigPrompt()
        {
            if (ActiveControl != null)
            {
                ActiveControl = null;
                ValidateChildren();
            }

            if (userOptions != null)
            {
                int startingModeState = btnStartingMode.GetStateToInt();
                int fullscreenModeState = btnFullscreenMode.GetStateToInt();
                bool startWindowed = startingModeState == 1 ? true : false;
                bool fullscreenExclusive = fullscreenModeState == 1 ? true : false;

                if (fullscreenExclusive && !startWindowed)
                {
                    userOptions.SetShortValue("Display", "Maximized", 0);
                    userOptions.SetStringValue("Display", "Mode", "Fullscreen");
                    userOptions.SetStringValue("Display", "FullscreenMode", "Fullscreen");
                }
                else if (fullscreenExclusive && startWindowed)
                {
                    userOptions.SetShortValue("Display", "Maximized", 0);
                    userOptions.SetStringValue("Display", "Mode", "Windowed");
                    userOptions.SetStringValue("Display", "FullscreenMode", "Fullscreen");
                }
                else if (!fullscreenExclusive && !startWindowed)
                {
                    userOptions.SetShortValue("Display", "Maximized", 1);
                    userOptions.SetStringValue("Display", "Mode", "WindowedFullscreen");
                    userOptions.SetStringValue("Display", "FullscreenMode", "Windowed");
                }
                else if (!fullscreenExclusive && startWindowed)
                {
                    userOptions.SetShortValue("Display", "Maximized", 1);
                    userOptions.SetStringValue("Display", "Mode", "Windowed");
                    userOptions.SetStringValue("Display", "FullscreenMode", "Windowed");
                }

                userOptions.SetStringValue("Display", "FullscreenWidth", txtResFullWidth.Text);
                userOptions.SetStringValue("Display", "FullscreenHeight", txtResFullHeight.Text);
                userOptions.SetStringValue("Display", "WindowedWidth", txtResWindWidth.Text);
                userOptions.SetStringValue("Display", "WindowedHeight", txtResWindHeight.Text);
                userOptions.SetStringValue("Display", "RefreshRate", txtRefreshRate.Text);

                userOptions.SetStringValue("Rendering", "OverallQuality", btnOverall.GetStateToString());
                userOptions.SetStringValue("Rendering", "TextureQuality", btnTextures.GetStateToString());
                userOptions.SetStringValue("Rendering", "EffectsQuality", btnEffects.GetStateToString());
                userOptions.SetStringValue("Rendering", "ModelQuality", btnModels.GetStateToString());
                userOptions.SetStringValue("Rendering", "FloraQuality", btnFlora.GetStateToString());
                userOptions.SetStringValue("Rendering", "SpeedTreeLOD", btnTreeQuality.GetStateToString());
                userOptions.SetStringValue("Rendering", "ParticleLOD", btnParticle.GetStateToString());
                userOptions.SetStringValue("Rendering", "ShadowQuality", btnShadows.GetStateToString());
                userOptions.SetStringValue("Rendering", "FogShadowsEnable", btnFogShadows.GetStateToString());
                userOptions.SetStringValue("Rendering", "LightingQuality", btnLightning.GetStateToString());
                userOptions.SetStringValue("Rendering", "InteriorLighting", btnInteriorLightning.GetStateToString());
                userOptions.SetStringValue("Rendering", "UseLod0a", btnUseLod0a.GetStateToString());
                userOptions.SetStringValue("Rendering", "AO", btnAmbientOcclusion.GetStateToString());
                userOptions.SetStringValue("Rendering", "VSync", btnVSync.GetStateToString());
                userOptions.SetStringValue("Rendering", "Smoothing", btnSmoothing.GetStateToString());
                userOptions.SetStringValue("Rendering", "UseDepthOfField", btnDepthOfField.GetStateToString());
                userOptions.SetStringValue("Rendering", "MotionBlur", btnMotionBlur.GetStateToString());

                userOptions.SetStringValue("Rendering", "MaximumFPS", txtMaxFPS.Text);
                userOptions.SetStringValue("Rendering", "RenderDistance", txtRenderDist.Text);
                userOptions.SetStringValue("Rendering", "VerticalFOV", txtFOV.Text);

                userOptions.SetStringValue("Rendering", "Gamma", (txtGamma.GetValue() / 100.0).ToString("0.000000", CultureInfo.InvariantCulture));
                userOptions.SetStringValue("Display", "HDPixelPlus", (txtRenderQuality.GetValue() / 100.0).ToString("0.000000", CultureInfo.InvariantCulture));

                userOptions.SetStringValue("General", "MouseSensitivity", txtMSensNormal.Text);
                userOptions.SetStringValue("General", "ADSMouseSensitivity", txtMSensADS.Text);
                userOptions.SetStringValue("General", "ScopedMouseSensitivity", txtMSensScoped.Text);
                userOptions.SetStringValue("General", "VehicleMouseSensitivity", txtMSensVehicle.Text);

                userOptions.SetStringValue("General", "MouseRawInput", btnMRawInput.GetStateToString());
                userOptions.SetStringValue("General", "ReduceInputLag", btnMReduceInputLag.GetStateToString());
                userOptions.SetStringValue("General", "MouseSmoothing", btnMSmoothing.GetStateToString());

                userOptions.SetStringValue("UI", "reticleShowCOF", chkShowCOF.GetState());
                userOptions.SetStringValue("UI", "reticleClassicMode", chkClassicReticle.GetState());
                userOptions.SetStringValue("UI", "LegacyHitmarker", chkLegacyHitmarker.GetState());
                userOptions.SetStringValue("UI", "CenterInventory", chkCenterInventory.GetState());

                userOptions.SetStringValue("Sound", "MuteAll", chkMuteAll.GetState());
                userOptions.SetStringValue("Sound", "Master", txtSoundMaster.Text);
                userOptions.SetStringValue("Sound", "SoundEffects", txtSoundEffects.Text);
                userOptions.SetStringValue("Sound", "UI", txtSoundUI.Text);
                userOptions.SetStringValue("Sound", "Dialog", txtSoundDialog.Text);
                userOptions.SetStringValue("Sound", "MusicMaster", txtMusicMaster.Text);
                userOptions.SetStringValue("Sound", "MusicAmbient", txtMusicAmbient.Text);
                userOptions.SetStringValue("Sound", "MusicEncounter", txtMusicEncounter.Text);

                userOptions.SaveConfig();
            }
        }

        public void UserHasChoosenResolution(ResolutionsForm.ResolutionInfo info)
        {
            txtResFullWidth.Text = info.Width.ToString();
            txtResFullHeight.Text = info.Height.ToString();
            txtRefreshRate.Text = info.RefreshRate.ToString();
        }

        private void checkBoxWindowedSameAsFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWindSameAsFull.Checked)
            {
                txtResWindWidth.SetTextSilent(txtResFullWidth.Text);
                txtResWindHeight.SetTextSilent(txtResFullHeight.Text);
                txtResWindWidth.Enabled = false;
                txtResWindHeight.Enabled = false;
            }
            else
            {
                txtResWindWidth.Enabled = true;
                txtResWindHeight.Enabled = true;
            }
        }

        private void textBoxResolutionFullscreenWidth_TextChanged(object sender, EventArgs e)
        {
            if (chkWindSameAsFull.Checked)
                txtResWindWidth.SetTextSilent(txtResFullWidth.Text);
        }

        private void textBoxResolutionFullscreenHeight_TextChanged(object sender, EventArgs e)
        {
            if (chkWindSameAsFull.Checked)
                txtResWindHeight.SetTextSilent(txtResFullHeight.Text);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveConfigPrompt();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonChooseResolution_Click(object sender, EventArgs e)
        {
            ResolutionsForm form = new ResolutionsForm(this);
            form.ShowDialog(this);
        }

        private void readOnlyToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (userOptions != null)
            {
                userOptions.ReadOnly = readOnlyToolStripMenuItem.Checked;
            }
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (conForm == null || conForm.IsDisposed)
                conForm = new ConsoleForm(this);
            
            conForm.Show();
            conForm.Focus();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setForm == null || setForm.IsDisposed)
                setForm = new SettingsForm(this);

            setForm.Show();
            setForm.Focus();
        }

        private void chkOneSensForAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOneSensForAll.Checked)
            {
                txtMSensADS.SetTextSilent(txtMSensNormal.Text);
                txtMSensScoped.SetTextSilent(txtMSensNormal.Text);
                txtMSensADS.Enabled = false;
                txtMSensScoped.Enabled = false;
            }
            else
            {
                txtMSensADS.Enabled = true;
                txtMSensScoped.Enabled = true;
            }
        }

        private void txtMSensNormal_TextChanged(object sender, EventArgs e)
        {
            if (chkOneSensForAll.Checked)
            {
                txtMSensADS.SetTextSilent(txtMSensNormal.Text);
                txtMSensScoped.SetTextSilent(txtMSensNormal.Text);
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUserOptions();
        }

        private void readOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userOptions != null)
                userOptions.SetReadOnlyFlag(readOnlyToolStripMenuItem.Checked);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
                aboutForm = new AboutForm(this);

            aboutForm.ShowDialog();
            aboutForm.Focus();
        }
    }
}
