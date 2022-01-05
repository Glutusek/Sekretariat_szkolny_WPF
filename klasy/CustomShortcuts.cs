using System.Windows;
using System.Windows.Input;

namespace Sekretariat_szkoły_WPF
{
    public static class CustomShortcuts
    {
        public static RoutedUICommand OpenStudents = new RoutedUICommand
            (
                "Otwórz uczniów",
                "OpenStudents",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.U, ModifierKeys.Control)
                }
            );

        public static RoutedUICommand OpenTeachers = new RoutedUICommand
            (
                "Otwórz nauczycieli",
                "OpenTeachers",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static RoutedUICommand OpenStaffMembers = new RoutedUICommand
            (
                "Otwórz pracowników obsługi",
                "OpenStaffMembers",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.P, ModifierKeys.Control)
                }
            );

        public static RoutedUICommand OpenShortcuts = new RoutedUICommand
            (
                "Otwórz edytor skrótów klawiszowych",
                "OpenShortcuts",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.E, ModifierKeys.Control)
                }
            );

        public static RoutedUICommand OpenAddStudent = new RoutedUICommand
            (
                "Otwórz dodawanie uczniów",
                "OpenAddStudent",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.U, ModifierKeys.Alt)
                }
            );

        public static RoutedUICommand OpenAddTeacher = new RoutedUICommand
            (
                "Otwórz dodawanie nauczycieli",
                "OpenAddTeacher",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Alt)
                }
            );

        public static RoutedUICommand OpenAddStaffMember = new RoutedUICommand
            (
                "Otwórz dodawanie pracowników obsługi",
                "OpenAddStaffMember",
                typeof(CustomShortcuts),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.P, ModifierKeys.Alt)
                }
            );
    }
}
