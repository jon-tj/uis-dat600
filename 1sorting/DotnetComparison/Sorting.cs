using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public abstract class Sorter
{
    public int Comparisons { get; protected set; }
    public int Swaps { get; protected set; }
    public float Milliseconds { get; protected set; }

    public void ResetStats()
    {
        Comparisons = 0;
        Swaps = 0;
        Milliseconds = 0;
    }

    public abstract void Sort(List<int> data);
}

public class InsertionSorter : Sorter
{
    public override void Sort(List<int> arr)
    {
        ResetStats();
        var sw = Stopwatch.StartNew();
        for (int i = 1; i < arr.Count; i++)
        {
            int key = arr[i];
            int j = i - 1;

            while (j >= 0)
            {
                Comparisons++;
                if (arr[j] <= key)
                    break;

                arr[j + 1] = arr[j];
                Swaps++;
                j--;
            }
            arr[j + 1] = key;
        }
        sw.Stop();
        Milliseconds = sw.Elapsed.Microseconds;
    }
}

public class MergeSorter : Sorter
{
    public override void Sort(List<int> arr)
    {
        ResetStats();
        var sw = Stopwatch.StartNew();
        MergeSort(arr, 0, arr.Count - 1);
        sw.Stop();
        Milliseconds = sw.Elapsed.Microseconds;
    }

    private void MergeSort(List<int> arr, int left, int right)
    {
        if (left >= right) return;

        int mid = (left + right) / 2;
        MergeSort(arr, left, mid);
        MergeSort(arr, mid + 1, right);
        Merge(arr, left, mid, right);
    }

    private void Merge(List<int> arr, int left, int mid, int right)
    {
        var L = arr.GetRange(left, mid - left + 1);
        var R = arr.GetRange(mid + 1, right - mid);

        int i = 0, j = 0, k = left;

        while (i < L.Count && j < R.Count)
        {
            Comparisons++;
            if (L[i] < R[j])
                arr[k++] = L[i++];
            else
                arr[k++] = R[j++];

            Swaps++;
        }

        while (i < L.Count)
        {
            arr[k++] = L[i++];
            Swaps++;
        }

        while (j < R.Count)
        {
            arr[k++] = R[j++];
            Swaps++;
        }
    }
}

public class HeapSorter : Sorter
{
    public override void Sort(List<int> arr)
    {
        ResetStats();
        var sw = Stopwatch.StartNew();
        int n = arr.Count;

        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(arr, n, i);

        for (int i = n - 1; i > 0; i--)
        {
            (arr[0], arr[i]) = (arr[i], arr[0]);
            Swaps++;
            Heapify(arr, i, 0);
        }
        sw.Stop();
        Milliseconds = sw.Elapsed.Microseconds;
    }

    private void Heapify(List<int> arr, int n, int i)
    {
        int largest = i;
        int l = 2 * i + 1;
        int r = 2 * i + 2;

        Comparisons++;
        if (l < n && arr[l] > arr[largest])
            largest = l;

        Comparisons++;
        if (r < n && arr[r] > arr[largest])
            largest = r;

        if (largest != i)
        {
            (arr[i], arr[largest]) = (arr[largest], arr[i]);
            Swaps++;
            Heapify(arr, n, largest);
        }
    }
}

public class QuickSorter : Sorter
{
    public override void Sort(List<int> data)
    {
        ResetStats();

        var sw = Stopwatch.StartNew();
        QuickSort(data, 0, data.Count - 1);
        sw.Stop();
        Milliseconds = sw.Elapsed.Microseconds;
    }

    private void QuickSort(List<int> arr, int low, int high)
    {
        if (low >= high)
            return;

        int p = Partition(arr, low, high);

        QuickSort(arr, low, p - 1);
        QuickSort(arr, p + 1, high);
    }

    private int Partition(List<int> arr, int low, int high)
    {
        // Pivot at first element to force worst case on sorted input
        int pivot = arr[low];

        while (low <= high)
        {
            while (arr[low] < pivot)
            {
                Comparisons++;
                low++;
            }

            while (arr[high] > pivot)
            {
                Comparisons++;
                high--;
            }

            if (low <= high)
            {
                if (low != high)
                {
                    (arr[low], arr[high]) = (arr[high], arr[low]);
                    Swaps++;
                }

                low++;
                high--;
            }
        }

        return low - 1;
    }

    private int Median(int a, int b, int c)
    {
        if (a < b)
            return (b < c) ? b : Math.Max(a, c);
        else
            return (a < c) ? a : Math.Max(b, c);
    }
}
