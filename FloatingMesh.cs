using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public partial class FloatingMesh : MonoBehaviour
{
    public enum CalculationProcess { Fast, Improved, Exact };
    public CalculationProcess calculationProcess = CalculationProcess.Fast;

    Mesh mesh;
    
    new Rigidbody rigidbody;

    [Header("Fluid Settings")]
    public float fluidLevel = 0f;
    [SerializeField]
    public float fluidDensity = 1f;
    [Header("Floating Body")]

    private float mass;
    private float volume;
    private float density;
    private float gravity;

    Vector3 buoyantForce;

    SubmergedPartCalculator calculator;
    float submergedVolume;
    Vector3 centroid;
    Vector3 submergedCentroid, transformedSubmergedCentroid;

    public float SubmergedVolume { get { return submergedVolume; } }
    public Vector3 SubmergedCentroid { get { return submergedCentroid; } }
   
    public void Refresh()
    {
        // In each FixedUpdate cycle, submerged volume and
        // submerged centroid are recalculated.
        //
        // This calculation process can be called outside of this class,
        // which is useful when testing.
        calculator.Calculate(out submergedVolume, out submergedCentroid);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        // Set strategy for submerged volume calculation
        switch (calculationProcess)
        {
            case CalculationProcess.Fast:
                calculator = new FastSPC(mesh, transform);
                break;
            case CalculationProcess.Improved:
                calculator = new ImprovedSPC(mesh, transform);
                break;
            case CalculationProcess.Exact:
                calculator = new ExactSPC(mesh, transform);
                break;
        }

        calculator.FluidLevel = fluidLevel;
        mass = rigidbody.mass;
        volume = calculator.Volume; 
        density = mass / volume;
        rigidbody.centerOfMass = calculator.Centroid;
        gravity = Physics.gravity.magnitude;
    }

    private void AddBuoyantForce()
    {
        if (submergedVolume == 0)
            return;
        // Calculate buoyant force
        // F_B = rho * V_B * g * (0,1,0)
        buoyantForce = Vector3.up * fluidDensity * submergedVolume * gravity;
        // Add buoyant force with built-in rigidbody method.
        // The reference point of this force is the transformed submerged centroid.
        rigidbody.AddForceAtPosition(buoyantForce, transformedSubmergedCentroid, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if (rigidbody.isKinematic)
            return;

        // Calculate submerged volume and submerged centroid
        Refresh();

        // Transform submerged centroid (from local coordinate system)
        // to world space
        transformedSubmergedCentroid = transform.TransformPoint(submergedCentroid);

        AddBuoyantForce();
    }
}
