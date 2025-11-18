using System;
using System.Windows.Forms;
using System.Drawing;

namespace QL_GiayTT.frm.Cls
{
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewNumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }

        private NumericUpDown numericUpDown = new NumericUpDown();
        public decimal Minimum
        {
            get { return numericUpDown.Minimum; }
            set { numericUpDown.Minimum = value; }
        }

        public decimal Maximum
        {
            get { return numericUpDown.Maximum; }
            set { numericUpDown.Maximum = value; }
        }

        public decimal Increment
        {
            get { return numericUpDown.Increment; }
            set { numericUpDown.Increment = value; }
        }

        public int DecimalPlaces
        {
            get { return numericUpDown.DecimalPlaces; }
            set { numericUpDown.DecimalPlaces = value; }
        }
    }

    public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        public DataGridViewNumericUpDownCell() : base()
        {
            this.Style.Format = "N0";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            
            if (this.DataGridView == null)
                return;
                
            DataGridViewNumericUpDownEditingControl numericUpDown = this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;
            if (numericUpDown != null)
            {
                numericUpDown.Minimum = 1;
                numericUpDown.Maximum = 999;
                numericUpDown.Increment = 1;
                numericUpDown.DecimalPlaces = 0;
                
                string value = initialFormattedValue as string;
                if (value != null && !string.IsNullOrWhiteSpace(value))
                {
                    value = value.Replace(",", "").Replace("đ", "").Trim();
                    if (int.TryParse(value, out int intValue) && intValue >= 1)
                    {
                        numericUpDown.Value = intValue;
                    }
                    else
                    {
                        numericUpDown.Value = 1;
                    }
                }
                else
                {
                    numericUpDown.Value = 1;
                }
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewNumericUpDownEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(int);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return 1;
            }
        }
    }

    public class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged = false;
        private int rowIndex;

        public DataGridViewNumericUpDownEditingControl()
        {
            this.Minimum = 1;
            this.Maximum = 999;
            this.Increment = 1;
            this.DecimalPlaces = 0;
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToString("N0");
            }
            set
            {
                try
                {
                    if (value is string)
                    {
                        string strValue = value.ToString().Replace(",", "").Replace("đ", "").Trim();
                        if (int.TryParse(strValue, out int intValue) && intValue >= 1)
                        {
                            this.Value = Math.Min(Math.Max(intValue, (int)this.Minimum), (int)this.Maximum);
                        }
                        else
                        {
                            this.Value = 1;
                        }
                    }
                    else if (value is int)
                    {
                        int intValue = (int)value;
                        this.Value = Math.Min(Math.Max(intValue, (int)this.Minimum), (int)this.Maximum);
                    }
                    else if (value is decimal)
                    {
                        int intValue = (int)(decimal)value;
                        this.Value = Math.Min(Math.Max(intValue, (int)this.Minimum), (int)this.Maximum);
                    }
                }
                catch
                {
                    this.Value = 1;
                }
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                this.Select(0, this.Text.Length);
            }
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            valueChanged = true;
            if (this.EditingControlDataGridView != null)
            {
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
            base.OnValueChanged(e);
        }
    }
}

