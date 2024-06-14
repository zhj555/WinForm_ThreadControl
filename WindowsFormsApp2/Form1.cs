using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        public Thread updateThread;
        public static int flag = 0;

        //1.声明委托
        public delegate void SetText(string text);

        //2.定义委托对象
        private SetText setText;

        //3.委托关联的方法
        void UpdateLabelText(string text)
        {
            label1.Text = text;
        }

        public Form1()
        {
            InitializeComponent();

            //4.委托绑定
            setText = UpdateLabelText;

            updateThread = new Thread(threadFun);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

#if false
        #region 一、直接调用方法，方法中使用委托，利用InvokeRequired属性

        private void threadFun()
        {
            Thread.Sleep(1000);  //不加会导致线程访问控件时InvokeRequired 属性为false,
                                // 导致异常原因是因为在创建窗口句柄之前，不能在控件上调用Invoke 或 BeginInvoke。
            while (true)
            {
                if (flag == 0)
                {
                    flag = 1;
                    updateText("hello");


                }
                else
                {
                    flag = 0;
                    updateText("schjjj");
                }


                Thread.Sleep(1000);
            }
        }


        private delegate void TestHandle(string str);         //声明一个委托

        private void updateText(string str)
        {

            try
            {
                //第一种写法
                /*                if (label1.InvokeRequired)
                                {
                                    TestHandle testHandle = new TestHandle(updateText);
                                    label1.Invoke(testHandle, str);
                                }
                                else
                                {
                                    label1.Text = str;
                                }*/


                //第二种写法
                /*                if (this.InvokeRequired)
                                {
                                    this.Invoke(new TestHandle(updateText), new object[] { str });
                                }
                                else
                                {
                                    this.label1.Text = str;
                                }*/

                //第三种写法
                if (label1.InvokeRequired)
                {
                    BeginInvoke(new Action<string>(x => { label1.Text = str; }), str);
                }
                else
                {
                    label1.Text = str;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"错误提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


            #endregion
#endif

        #region 二、调用委托，委托进一步关联方法 
        //1、使用委托 (最基础的调用方法：委托五步法)

#if false
        public void threadFun()
        {
            c

            //错误调用委托【报错：System.InvalidOperationException:“线程间操作无效: 从不是创建控件“Form1”的线程访问它。”】
            //因为想要在多线程里操作主线程的控件，你还得经过控件的同意【使用.Invoke()方法】
            //setText();

            //【05】正确调用委托
            this.Invoke(setText,"跨线程操作控件成功");

        }
#endif

        //2、使用Action作为委托来创建
        //微软从某个版本开始，出来了Action 和 Lamda 表达式，Action是系统委托，也就是说不用手动创建委托，它有个兄弟叫Func
        //Action 没有返回值，最多有16个参数
        //Func 必须要有返回值，最多有16个参数，最后一个参数表示返回值

        //① 第一步简化：用Action 作为委托来创建
#if false
        public void threadFun()
        {
            Thread.Sleep(1000);

            //1.创建委托和绑定委托
            Action action = new Action(UpdataText);

            //2.调用委托
            this.Invoke(action);
        }

        private void UpdataText() 
        {
            label1.Text= "text";
        }
#endif
        //② 第二步简化：委托对象只用一次，所以可以直接放到参数里
#if false
        public void threadFun()
        {
            Thread.Sleep(1000);

            //创建委托、绑定委托、调用委托
            this.Invoke(new Action(UpdataText));
        }
        private void UpdataText()
        {
            label1.Text = "kkk";
        }
#endif
        //③ 第三步简化: 用Lamda表达式代替方法 （推荐使用）
        public void threadFun()
        {
            Thread.Sleep(1000);

            this.Invoke(new Action(() =>
            {
                label1.Text = "Lamda简化替代方法";
            }));
        }

        #endregion





    }
}

