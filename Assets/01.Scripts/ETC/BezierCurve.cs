using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    private static BezierCurve curve;
    public static void CalculateCurvePoints(int count, Transform[] points)
    {
        if(curve == null)
            curve = new BezierCurve();

        Vector3[] curvePoints;
        if (points == null || points.Length < 2) return;

        curvePoints = new Vector3[count + 1];
        float unit = 1.0f / count;

        ref Transform[] P = ref points;

        int n = P.Length - 1;
        int[] C = curve.GetCombinationValues(n); // nCi
        float[] T = new float[n + 1];      // t^i
        float[] U = new float[n + 1];      // (1-t)^i

        // Iterate curvePoints : 0 ~ count(200)
        int k = 0; float t = 0f;
        for (; k < count + 1; k++, t += unit)
        {
            curvePoints[k] = Vector3.zero;

            T[0] = 1f;
            U[0] = 1f;
            T[1] = t;
            U[1] = 1f - t;

            // T[i] = t^i
            // U[i] = (1 - t)^i
            for (int i = 2; i <= n; i++)
            {
                T[i] = T[i - 1] * T[1];
                U[i] = U[i - 1] * U[1];
            }

            // Iterate Bezier Points : 0 ~ n(number of points - 1)
            for (int i = 0; i <= n; i++)
            {
                curvePoints[k] += C[i] * T[i] * U[n - i] * P[i].position;
            }
        }
    }

    private int[] GetCombinationValues(int n)
    {
        int[] arr = new int[n + 1];

        for (int r = 0; r <= n; r++)
        {
            arr[r] = Combination(n, r);
        }
        return arr;
    }

    private int Factorial(int n)
    {
        if (n == 0 || n == 1) return 1;
        if (n == 2) return 2;

        int result = n;
        for (int i = n - 1; i > 1; i--)
        {
            result *= i;
        }
        return result;
    }

    private int Permutation(int n, int r)
    {
        if (r == 0) return 1;
        if (r == 1) return n;

        int result = n;
        int end = n - r + 1;
        for (int i = n - 1; i >= end; i--)
        {
            result *= i;
        }
        return result;
    }

    private int Combination(int n, int r)
    {
        if (n == r) return 1;
        if (r == 0) return 1;

        // C(n, r) == C(n, n - r)
        if (n - r < r)
            r = n - r;

        return Permutation(n, r) / Factorial(r);
    }

    // 블로그 참고 : https://rito15.github.io/posts/unity-study-bezier-curve/
}
