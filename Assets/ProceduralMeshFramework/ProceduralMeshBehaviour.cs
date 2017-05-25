using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ProceduralMeshRenderer))]
public class ProceduralMeshBehaviour : MonoBehaviour
{
    public ProceduralMeshRenderer PMR { get; private set; }
    private ProceduralMesh PM { get; set; }
    protected ProceduralMeshBuilder PMB { get; private set; }
    public void Awake()
    {
        PMR = GetComponent<ProceduralMeshRenderer>();
        PM = new ProceduralMesh();
        PMB = new ProceduralMeshBuilder();
        PMR.Mesh = PM;

        Initialize();
    }
    protected virtual void Initialize()
    {

    }

    public bool
        Regenerate = false,
        UseAsync = true,
        Recompile = false,
        ImplicitRecompile = true;
    [SerializeField]
    private bool asyncRunning;

    public bool AsyncRunning { get { return asyncRunning; } protected set { asyncRunning = value; } }

    public void Update()
    {

        if (Regenerate)
            if (!UseAsync) //Dont use async
            {
                Regenerate = false;
                GenerateMesh();
                if (ImplicitRecompile)
                    Recompile = true;
            }
            else if (!AsyncRunning) //Use Async, but dont run if we are still running.
            {
                StartCoroutine(PreAsyncGenerateMesh());
                Regenerate = false;
            }
        if (Recompile)
        {
            CompileMesh();
            Recompile = false;
        }
    }

    void CompileMesh()
    {
        PMB.Compile(PM);
        //PMR.ForceUpdate();
    }
    protected virtual void GenerateMesh()
    {
        throw new System.NotImplementedException("GenerateMesh has not been provided for " + GetType().ToString());
    }
    private IEnumerator PreAsyncGenerateMesh()
    {
        yield return StartCoroutine(AsyncGenerateMesh());
        if (ImplicitRecompile)
            Recompile = true;
        yield return null;
    }
    protected virtual IEnumerator AsyncGenerateMesh()
    {
        throw new System.NotImplementedException("AsyncGenerateMesh has not been provided for " + GetType().ToString());
    }

}
