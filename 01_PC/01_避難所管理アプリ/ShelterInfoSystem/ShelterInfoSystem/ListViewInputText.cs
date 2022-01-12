using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

public class ListViewInputText : TextBox
{
    public class InputEventArgs : EventArgs
    {
        public string Path = "";
        public string NewName = "";
        public int listItemNo = 0;
        public int listSubItemNo = 0;
    }

    public delegate void InputEventHandler(object sender, InputEventArgs e);

    //イベントデリゲートの宣言
    public event InputEventHandler FinishInput;

    private InputEventArgs args = new InputEventArgs();
    private bool finished = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent">対象となるListViewコントロール</param>
    /// <param name="item">編集対象のアイテム</param>
    /// <param name="subitem_index">編集する対象の列</param>
    public ListViewInputText(ListView parent, ListViewItem item, int subitem_index, int nMxLen) : base()
    {
        args.Path = item.SubItems[0].Text;
        args.NewName = item.SubItems[1].Text;
        args.listItemNo = item.Index;
        args.listSubItemNo = subitem_index;

        int left = 0;
        for (int i = 0; i < subitem_index; i++)
        {
          left += parent.Columns[i].Width;
        }
        int width = item.SubItems[subitem_index].Bounds.Width;
        int height = item.SubItems[subitem_index].Bounds.Height - 4;

        this.Parent = parent;
        this.Size = new Size(width, height);
        this.Left = left;
        this.Top = item.Position.Y - 1;
        this.Text = item.SubItems[subitem_index].Text;
        this.LostFocus += new EventHandler(textbox_LostFocus);
        this.ImeMode = ImeMode.NoControl;
        this.Multiline = false;
        this.KeyDown += new KeyEventHandler(textbox_KeyDown);
        this.Focus();
        // ● 4/14
        this.KeyPress += new KeyPressEventHandler(textbox_KeyPress);
        this.MaxLength = nMxLen;
    }

    void Finish(string new_name)
    {
        // Enterで入力を完了した場合はKeyDownが呼ばれた後に
        // さらにLostFocusが呼ばれるため，二回Finishが呼ばれる
        if (!finished)
        {
            // textbox.Hide()すると同時にLostFocusするため，
            // finished=trueを先に呼び出しておかないと，
            // このブロックが二回呼ばれてしまう．
            finished = true;
            this.Hide();
            args.NewName = new_name;
            FinishInput(this, args);
        }
    }

    void textbox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            Finish(this.Text);
        }
        else if (e.KeyCode == Keys.Escape)
        {
            Finish(args.NewName);
        }
    }

    void textbox_LostFocus(object sender, EventArgs e)
    {
        Finish(this.Text);
    }

    // ● 4/14
    // 数値入力 
    void textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      // 制御文字は入力可
      if (char.IsControl(e.KeyChar))
      {
          e.Handled = false;
          return;
      }


      // 数字(0-9)は入力可
      if (e.KeyChar < '0' || '9' < e.KeyChar)
      {
          // 上記以外は入力不可
          e.Handled = true;
      }


      //// 数字(0-9)は入力可
      //if (char.IsDigit(e.KeyChar))
      //{
      //    e.Handled = false;
      //    return;
      //}
      //// 小数点は１つだけ入力可
      //if (e.KeyChar == '.')
      //{
      //    TextBox target = sender as TextBox;
      //    if (target.Text.IndexOf('.') < 0)
      //    {
      //        // 複数のピリオド入力はNG
      //        e.Handled = false;
      //        return;
      //    }
      //}



    }
    
}

