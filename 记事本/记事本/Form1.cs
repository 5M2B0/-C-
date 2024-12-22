using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 记事本
{
    public partial class 记事本 : Form
    {
        private string currentFilePath = "";
        private bool isTextChanged = false;
        private Stack<string> undoStack = new Stack<string>(); // 撤销栈
        private Stack<string> redoStack = new Stack<string>(); // 重做栈
        public 记事本()
        {
            InitializeComponent();
            UpdateStatusStrip();
            // 初始设置
            textBox1.WordWrap = true;
            this.Text = "记事本";
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            // 在窗体加载时更新 StatusStrip
            UpdateStatusStrip();
        }

        private void UpdateStatusStrip()
        {
            // 获取当前时间
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            // 设置 StatusStrip 上的文本
            toolStripStatusLabel1.Text = $"欢迎来到汤汤记事本，当前时间是：{currentTime}";
        }
        private bool PromptSaveChanges()
        {
            // 显示提示框，询问用户是否保存未保存的更改
            var result = MessageBox.Show("该文件你还未保存，确定退出吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // 如果用户选择“是”，返回 true；否则返回 false
            return result == DialogResult.Yes;
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    File.WriteAllText(currentFilePath, textBox1.Text);
                    isTextChanged = false;
                    this.Text = $"记事本 - {Path.GetFileName(currentFilePath)}";
                }
            }
        }



        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 如果文本已被修改且用户选择不保存更改，则退出方法
            if (isTextChanged && !PromptSaveChanges())
                return;

            // 清空文本框中的内容
            textBox1.Clear();

            // 重置文件路径
            currentFilePath = "";

            // 重置文本修改标记
            isTextChanged = false;

            // 更新窗体标题，表示当前编辑的是一个新文件
            this.Text = "记事本 - 未命名";
        }

        private void 打开OCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 如果文本已被修改且用户选择不保存更改，则退出方法
            if (isTextChanged && !PromptSaveChanges())
                return;

            // 创建并配置打开文件对话框
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                // 显示对话框并检查用户是否选择了文件
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新当前文件路径
                    currentFilePath = openFileDialog.FileName;

                    // 读取文件内容并显示在文本框中
                    textBox1.Text = File.ReadAllText(currentFilePath);

                    // 重置文本修改标记
                    isTextChanged = false;

                    // 更新窗体标题，表示当前编辑的文件名
                    this.Text = $"记事本 - {Path.GetFileName(currentFilePath)}";
                }
            }
        }

        private void 保存SCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 如果当前文件路径为空，调用另存为功能
            if (string.IsNullOrEmpty(currentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                // 将文本框中的内容写入当前文件
                File.WriteAllText(currentFilePath, textBox1.Text);

                // 重置文本修改标记
                isTextChanged = false;
            }
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建并配置另存为文件对话框
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                // 显示对话框并检查用户是否选择了文件
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新当前文件路径
                    currentFilePath = saveFileDialog.FileName;

                    // 将文本框中的内容写入指定文件
                    File.WriteAllText(currentFilePath, textBox1.Text);

                    // 重置文本修改标记
                    isTextChanged = false;

                    // 更新窗体标题，表示当前编辑的文件名
                    this.Text = $"记事本 - {Path.GetFileName(currentFilePath)}";
                }
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 如果文本已被修改且用户选择不保存更改，则退出方法
            if (isTextChanged && !PromptSaveChanges())
                return;

            // 退出应用程序
            Application.Exit();
        }

        private void 撤销UCtrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 如果文本框支持撤销操作，则执行撤销
            if (textBox1.CanUndo)
            {
                textBox1.Undo();

                // 标记文本已被修改
                isTextChanged = true;
            }
        }

        private void 剪切TCtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 执行剪切操作，将选中的文本剪切到剪贴板
            textBox1.Cut();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void 复制CCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 执行粘贴操作，从剪贴板粘贴文本到文本框
            textBox1.Paste();
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 选中文本框中的所有文本
            textBox1.SelectAll();
        }

        

        private void 字体ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建并配置字体对话框
            using (FontDialog fontDialog = new FontDialog())
            {
                // 将对话框的字体设置为当前文本框的字体
                fontDialog.Font = textBox1.Font;

                // 显示对话框并检查用户是否选择了字体
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新文本框的字体为用户选择的字体
                    textBox1.Font = fontDialog.Font;
                }
            }   
        }

        private void 字体颜色ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建并配置颜色对话框
            using (ColorDialog colorDialog = new ColorDialog())
            {
                // 将对话框的颜色设置为当前文本框的前景色
                colorDialog.Color = textBox1.ForeColor;

                // 显示对话框并检查用户是否选择了颜色
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新文本框的前景色为用户选择的颜色
                    textBox1.ForeColor = colorDialog.Color;
                }
            }
        }

        private void 重复RCtrlYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查重做栈是否有内容
            if (redoStack.Count > 0)
            {
                // 将当前文本推送到撤销栈
                undoStack.Push(textBox1.Text);

                // 从重做栈中弹出并设置文本
                textBox1.Text = redoStack.Pop();
                isTextChanged = true;
            }
        }

        private void 工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // 如果文本已被修改且用户选择不保存更改，则退出方法
            if (isTextChanged && !PromptSaveChanges())
                return;

            // 清空文本框中的内容
            textBox1.Clear();

            // 重置文件路径
            currentFilePath = "";

            // 重置文本修改标记
            isTextChanged = false;

            // 更新窗体标题，表示当前编辑的是一个新文件
            this.Text = "记事本 - 未命名";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // 如果文本已被修改且用户选择不保存更改，则退出方法
            if (isTextChanged && !PromptSaveChanges())
                return;

            // 创建并配置打开文件对话框
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                // 显示对话框并检查用户是否选择了文件
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新当前文件路径
                    currentFilePath = openFileDialog.FileName;

                    // 读取文件内容并显示在文本框中
                    textBox1.Text = File.ReadAllText(currentFilePath);

                    // 重置文本修改标记
                    isTextChanged = false;

                    // 更新窗体标题，表示当前编辑的文件名
                    this.Text = $"记事本 - {Path.GetFileName(currentFilePath)}";
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // 如果当前文件路径为空，调用另存为功能
            if (string.IsNullOrEmpty(currentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                // 将文本框中的内容写入当前文件
                File.WriteAllText(currentFilePath, textBox1.Text);

                // 重置文本修改标记
                isTextChanged = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void 关于记事本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你逗我的吧，你怎么不会呢，不会就百度啊！", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 关于制作者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请联系：3508044910@qq.com", "关于作者", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}