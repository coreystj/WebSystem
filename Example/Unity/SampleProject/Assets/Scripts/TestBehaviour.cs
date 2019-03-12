using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSystem.Example.SampleProject.Models;
using WebSystem.MySQLUnity.Repositories;

public class TestBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var model = new SampleTable(-1, "Hello World");
        Repository.Create("http://localhost:54904/", model, OnCreateSuccess, OnCreateFailed);
    }

    private void OnCreateFailed(Exception ex)
    {
        Debug.Log(ex);
    }

    private void OnCreateSuccess(SampleTable model)
    {
        Debug.Log(string.Format("Create sample table with id: {0}", model.Id));
    }
}
