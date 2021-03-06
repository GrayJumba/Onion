﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AcademicsDesk.Pages
{
    /// <summary>
    /// Interaction logic for SubjectSelection.xaml
    /// </summary>
    public partial class SeniorSubjectSelection : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public SeniorSubjectSelection()
        {
            InitializeComponent();
            SubjectSelectiontDg.DataContext = MySQLHandler.SeniorSubjectSelection.Default.Dt;
            advancedSubjectPicker.SetFilter(1, -1, 0);
            advancedSubjectPicker.subjectPicker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            studentPicker.SelectedStudentsChanged += studentPicker_SelectedStudentsChanged;
            studentSelectionDg.IsVisibleChanged += SeniorSubjectSelection_IsVisibleChanged;
            studentSelectionDg.DataContext = MySQLHandler.StudentSeniorSubjectSelection.Default.Dt;
            studentSelectionDg.IsVisibleChanged += studentSelectionDg_IsVisibleChanged;
            studentPicker2.SelectedStudentsChanged += studentPicker2_SelectedStudentsChanged;

        }
        #region tabular tab
        void SeniorSubjectSelection_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommandBinding_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed += SaveCommandBinding_Executed;        
                MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommandBinding_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommandBinding_Executed;
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommandBinding_Executed;

            }
        }
        void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintGrid(SubjectSelectiontDg, "");
        }

        void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SubjectSelectiontDg.HasItems)
                e.CanExecute = true;
            e.CanExecute = false;
        }
        void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.SeniorSubjectSelection.Default.saveChanges())
                MessageBox.Show("Saved");
            else
                MessageBox.Show("Not Saved");
         }

        void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void studentPicker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.SeniorSubjectSelection.Default.refreshDt(studentPicker.SelectedStudentAutoID, studentPicker.SelectedStreamAutoID, studentPicker.SelectedClassOf,advancedSubjectPicker.subjectPicker.selected_subject_code);
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        #endregion


        #region singular tab
        void studentPicker2_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            MySQLHandler.StudentSeniorSubjectSelection.Default.refreshDt(studentPicker2.SelectedStudentAutoID);

        }
        void studentSelectionDg_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommandBinding_CanExecute2;
                MainWindow.Default.SaveCommandBinding.Executed += SaveCommandBinding_Executed2;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommandBinding_CanExecute2;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommandBinding_Executed2;
            }
        }
        void SaveCommandBinding_Executed2(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.StudentSeniorSubjectSelection.Default.saveChanges();
        }

        void SaveCommandBinding_CanExecute2(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MySQLHandler.StudentSeniorSubjectSelection.Default.Dt.GetChanges() == null)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        #endregion

        private void SubjectSelectiontDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void SubjectSelectiontDg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }
    }
}
