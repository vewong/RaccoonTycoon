﻿using UnityEngine;
            Debug.Log("Opening " + solutionPath);
            System.Diagnostics.Process.Start(solutionPath);
            Debug.LogError("Solution not found at " + solutionPath);