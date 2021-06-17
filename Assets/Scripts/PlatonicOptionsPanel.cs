using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ProceduralGraph;
using ProceduralMeshFramework;
using UnityEngine;
using UnityEngine.UI;

public class PlatonicOptionsPanel : MonoBehaviour
{
    [SerializeField] private ProceduralPlatonicMesh _ppm;
    [SerializeField] private InputField _seed;
    [SerializeField] private InputField _solidity;
    [SerializeField] private Slider _soliditySlider;
    [SerializeField] private Dropdown _shapeSelection;
    [SerializeField] private InputField _subdivisions;
    [SerializeField] private InputField _minHeight;
    [SerializeField] private InputField _maxHeight;
    [SerializeField] private Toggle _slerp;
    [SerializeField] private Button _generate;

    private void Awake()
    {
        if(_seed != null)
            _seed.onEndEdit.AddListener(SeedDone);
        if (_solidity != null)
            _solidity.onEndEdit.AddListener(SoliditySliderDone);
        if (_soliditySlider != null)
            _soliditySlider.onValueChanged.AddListener(SoliditySliderChanged);
        if (_shapeSelection != null)
            _shapeSelection.onValueChanged.AddListener(ShapeChanged);
        if (_subdivisions != null)
            _subdivisions.onEndEdit.AddListener(SubdivisionsChanged);
        if (_minHeight != null)
            _minHeight.onEndEdit.AddListener(MinHeightChanged);
        if (_maxHeight != null)
            _maxHeight.onEndEdit.AddListener(MaxHeightChanged);
        if(_slerp != null)
            _slerp.onValueChanged.AddListener(SlerpChanged);
        if (_generate != null)
            _generate.onClick.AddListener(GenerateClicked);

        Fix();
    }

    private void Fix()
    {
        //_seed.text = _ppm.PlanetParameters.Seed.ToString();
        //UpdateSolidity(_ppm.MeshParameters.Solidity);
        _shapeSelection.value = ConvertShapeType(_ppm.GraphParameters.Shape);
        //_subdivisions.text = _ppm.GraphParameters.Subdivisions.ToString();
        //_minHeight.text = _ppm.PlanetParameters.MinHeight.ToString();
        //_maxHeight.text = _ppm.PlanetParameters.MaxHeight.ToString();
        //_slerp.isOn = _ppm.GraphParameters.Slerp;
    }

    private void GenerateClicked()
    {
        _ppm.Regenerate = true;
    }

    private void SlerpChanged(bool arg0)
    {
        _ppm.GraphParameters.Slerp = arg0;
    }

    private void MaxHeightChanged(string arg0)
    {
        int value;
        if (!int.TryParse(arg0, out value))
            value = _ppm.PlanetParameters.MaxHeight;
        _ppm.PlanetParameters.MaxHeight = value;
        _subdivisions.text = _ppm.PlanetParameters.MaxHeight.ToString();
    }

    private void MinHeightChanged(string arg0)
    {
        int value;
        if (!int.TryParse(arg0, out value))
            value = _ppm.PlanetParameters.MinHeight;
        _ppm.PlanetParameters.MinHeight = value;
        _subdivisions.text = _ppm.PlanetParameters.MinHeight.ToString();
    }

    private void SubdivisionsChanged(string arg0)
    {
        int value;
        if (!int.TryParse(arg0, out value))
            value = _ppm.GraphParameters.Subdivisions;
        _ppm.GraphParameters.Subdivisions = Mathf.Max(0, value);
        _subdivisions.text = _ppm.GraphParameters.Subdivisions.ToString();
    }

    private int ConvertShapeType(ShapeType shape)
    {
        //Unfortunately I have to manually define options
        switch (shape)
        {
            case ShapeType.Dodecahedron:
                return 0;

            case ShapeType.Icosahedron:
                return 1;
            case ShapeType.Octahedron:
                return 2;
            case ShapeType.Cube:
                return 3;
            case ShapeType.Tetrahedron:
                return 4;
            default:
                throw new ArgumentException("Argument does not correspond to shape type!", "shape");
        }

        _ppm.GraphParameters.Shape = shape;
    }

    private void ShapeChanged(int arg0)
    {
        //Unfortunately I have to manually define options
        ShapeType shape;
        switch (arg0)
        {
            case 0:
                shape = ShapeType.Dodecahedron;
                break;

            case 1:
                shape = ShapeType.Icosahedron;
                break;

            case 2:
                shape = ShapeType.Octahedron;
                break;

            case 3:
                shape = ShapeType.Cube;
                break;
            case 4:
                shape = ShapeType.Tetrahedron;
                break;
            default:
                throw new ArgumentException("Argument does not correspond to shape type!", "arg0");
        }

        _ppm.GraphParameters.Shape = shape;
    }

    private void SoliditySliderChanged(float arg0)
    {
        float value = RoundToPlace(arg0, 2);
        UpdateSolidity(value);
    }

    private float RoundToPlace(float value, int place = 0)
    {
        var scale = Mathf.Pow(10f, place);
        return Mathf.RoundToInt(value * scale) / scale;
    }

    private void SoliditySliderDone(string arg0)
    {
        float value;
        if (float.TryParse(arg0, out value))
        {
            value = RoundToPlace(value, 0) / 100f;
        }
        else
        {
            value = _ppm.MeshParameters.Solidity;
        }

        UpdateSolidity(value);
    }


    private void UpdateSolidity(float normal)
    {
        _ppm.MeshParameters.Solidity = normal;
        _solidity.text = Mathf.RoundToInt(normal * 100).ToString(CultureInfo.InvariantCulture);
        _soliditySlider.normalizedValue = normal;
    }

    private void SeedDone(string arg0)
    {
        int seed;
        if (!int.TryParse(arg0, out seed))
        {
            seed = arg0.GetHashCode();
            arg0 = seed.ToString();
        }

        _ppm.PlanetParameters.Seed = seed;
        _seed.text = arg0;
    }
}