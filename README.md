# ProcessList
WPF application using MVVM (Model-View-ViewModel) pattern which displays list of processes in operating systems and provides interaction with them. It shows concepts such as UserControl, DataTemplate and Command (to isolate model from presentation).

The list can be refreshed automatically after a configurable time or at the user's request. It is possible to sort and filter processes. After clicking on a selected process, its details are displayed, such as ID, priority, number of threads, memory and CPU usage, as well as a list of modules that have been loaded by this process. Additionally, it is possible to change the priority of each process and kill it (if it is not possible for a given process, an appropriate error message is displayed).
