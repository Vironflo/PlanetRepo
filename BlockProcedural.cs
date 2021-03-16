using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BlockProcedural : MonoBehaviour
{
	private Color32[] colors;
	Color32[] MeshCol;
	public Color32 grassColor;
	public Color emissionCol;

	private Material tempMat;
	private float HeighBlockValue = 0.35f;
	private float HeightDirection = 0.72f;
	MeshRenderer meshRender;
	Mesh mesh;
	private float EdgesCenterIntensity;

	List<int> nbpoint=new List<int>();
	public Vector3[] vertices;
	public float[] EdgeList;
	int widhtMap_P;
	int heightMap_p;

	public void initVerticesOfMesh()
	{

		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(2, 0, 0);
		vertices[2] = new Vector3(2, 2, 0);//2
		vertices[3] = new Vector3(1, 2, 0);//3
		vertices[4] = new Vector3(0, 2, 0);//4
		vertices[5] = new Vector3(0, 2, 1);//5
		vertices[6] = new Vector3(0, 2, 2);//6
		vertices[7] = new Vector3(1, 2, 2);//7
		vertices[8] = new Vector3(2, 2, 2);//8
		vertices[9] = new Vector3(2, 2, 1);//9
		vertices[10] = new Vector3(1, 2, 1);
		vertices[11] = new Vector3(2, 0, 2);
		vertices[12] = new Vector3(0, 0, 2);
	}
	public void setHeightDirection(float HeighDir)
	{
		this.HeightDirection = HeighDir;
	}
	private float getHeighDirection()
	{
		return this.HeightDirection;
	}
	public int[] InitTrianglesOfMesh()
	{
		int[] triangles = {
		0,4,3,
		3,2,1,
		1,0,3,
		3,4,5,
		5,10,3,
		5,6,7,
		7,10,5,
		7,8,9,
		9,10,7,
		9,2,3,
		3,10,9,
		8,11,9,
		9,1,2,
		9,11,1,
		0,5,4,
		5,12,6,
		12,5,0,
		7,6,12,
		12,11,7,
		7,11,8,
		0,1,11,
		11,12,0,
		};
		return triangles;
	}

	public void CreateCube()
	{
		initVerticesOfMesh();


		MeshRenderer meshRender = this.gameObject.AddComponent<MeshRenderer>();
		tempMat = meshRender.sharedMaterial;
		meshRender.material = tempMat;
		mesh.RecalculateNormals();

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = InitTrianglesOfMesh();

	}
    #region ModifyTrigger
    
    public void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.layer == 8)
		{
			if(gameObject.layer!=8)
			{
				Vector3 currentBlockPos = collision.gameObject.transform.position;
				Vector3 currentCheckedBlockPos = this.gameObject.transform.position;
				ColliderTriggerCheckAroundHole(currentBlockPos, currentCheckedBlockPos);
			}
		}
	}
		public void ColliderTriggerCheckAroundHole(Vector3 currentBlock, Vector3 checkedBlock)
		{
		
		float DifferenceX;
		float DifferenceY;
		float DifferenceZ;
		DifferenceX = currentBlock.x + 0.5f - checkedBlock.x;
		DifferenceY = currentBlock.y - checkedBlock.y;
		DifferenceZ = currentBlock.z + 0.5f - checkedBlock.z ;
		meshRender = this.gameObject.GetComponent<MeshRenderer>();
		tempMat = new Material(meshRender.sharedMaterial);
		if (DifferenceX < 0)
		{
			if (DifferenceZ > 0)
			{
				nbpoint.Add(6);
			}
			 if (DifferenceZ == 0)
			{
				NbPointAdd(4, 5, 6, nbpoint);
			}
			 if (DifferenceZ < 0)
			{
				nbpoint.Add(4);
			}
		}
		if (DifferenceX > 0)
		{
			if (DifferenceZ > 0)
			{
				nbpoint.Add(8);
			}
			else if (DifferenceZ == 0)
			{
				NbPointAdd(2, 9, 8, nbpoint);
				
			}
			else if (DifferenceZ < 0)
			{
				nbpoint.Add(2);
			}

		}
		if (DifferenceX == 0)
		{
			if (DifferenceZ > 0)
			{
				NbPointAdd(6, 7, 8, nbpoint);
			}
			else if (DifferenceZ < 0)
			{
				NbPointAdd(4, 3, 2, nbpoint);
				
			}

		}
	
		if(CountNbPointForAjustCenter())
		{
			nbpoint.Add(10);
		

		}
		setOrderEdgeTop(nbpoint, (float)Random.Range(0, 0.9f));
		AsGroundBlock();//le vide
	}
	#endregion
	public bool CountNbPointForAjustCenter()
	{
		if (nbpoint.Count >= 4)
		{
			EdgesCenterIntensity = -0.2f;
			return true;
		}
		else if (nbpoint.Count >= 5)
		{
			EdgesCenterIntensity = 0.4f;
			return true;
		}
		else if (nbpoint.Count==0)
		{
			EdgesCenterIntensity = 0.3f;
			return true;
		}
		else
		{
			return false;
		}
	}
	public List<int> NbPointAdd(int x, int y, int z, List<int> nbpoint)
	{
		if(x!=0)
		{
			nbpoint.Add(x);
		}
		if(y!=0)
		{
			nbpoint.Add(y);
		}
		if(z!=0)
		{
			nbpoint.Add(z);
		}
		return nbpoint;
	}
	public List<int> SelectingVertToColorForTop(Vector3 currentPos, List<int> nbpoint, int currentHeigh)//, List<int> additionalPoint)
	{
		if (currentPos.x == 0)
		{
			NbPointAdd(4, 5, 6,nbpoint);
		}
		if (currentPos.z == currentHeigh)
		{
			NbPointAdd(6, 7, 8, nbpoint);
		}
		if (currentPos.x == currentHeigh)
		{
			NbPointAdd(2, 9, 8, nbpoint);
		}
		if (currentPos.z == 0)
		{
			NbPointAdd(4, 3, 2, nbpoint);
		}

		return  nbpoint;
	}
	
	public void DestroyMeshRenderer()
	{
		Vector3 pos = new Vector3();
		this.gameObject.transform.localScale = new Vector3(1,1,1);	
		this.gameObject.GetComponent<BoxCollider>();
		pos = this.gameObject.transform.position;
		pos.x -= 0.5f;
		pos.z -= 0.5f;
		pos.y = 6.6f;
		this.gameObject.transform.position = pos;
		meshRender = this.gameObject.GetComponent<MeshRenderer>();
		meshRender.enabled = false;	
	}

	public void FixFloor2height()
	{
		meshRender = this.gameObject.GetComponent<MeshRenderer>();
		tempMat = new Material(meshRender.sharedMaterial);
		tempMat.SetFloat("Vector1_B4969D56", 0.87f);
		MeshFilter meshFl = GetComponent<MeshFilter>();
		Mesh meshCopy = Mesh.Instantiate(meshFl.sharedMesh);

		mesh = meshFl.mesh = meshCopy;	
		meshRender.material = tempMat;
	}
	public void SetHeighBlockValue()
	{
		HeighBlockValue = 0.4f; // moutain
	}
	
	public void IndiceOfEdge(int direction)
	{
		meshRender = this.gameObject.GetComponent<MeshRenderer>();
		tempMat = new Material(meshRender.sharedMaterial);
		switch (direction)
		{
			case 0:
				nbpoint.Add(4);
				break;
			case 1:
				NbPointAdd(4, 5, 6, nbpoint);					
				break;
			case 2:
				nbpoint.Add(6);
				break;
			case 3:
				NbPointAdd(4, 3, 2, nbpoint);
				break;
			case 4:
				//center = nothing ou detruire block
				break;
			case 5:
				NbPointAdd(6, 7, 8, nbpoint);
				break;
			case 6:
				nbpoint.Add(2);
				break;
			case 7:
				NbPointAdd(2, 9, 8, nbpoint);

				break;
			case 8:
				nbpoint.Add(8);
				break;
		}
		setOrderEdgeTop(nbpoint, (float)Random.Range(0, 0.9f));
		AsGroundBlock();
	}
	public void SetBorderBezierByColoring(GameObject topblock, int currentBlock, int mapsize, float AdditiveInt)
	{
		meshRender = this.gameObject.GetComponent<MeshRenderer>();
		tempMat = new Material(meshRender.sharedMaterial);

		setOrderEdgeTop(SelectingVertToColorForTop(topblock.transform.position, nbpoint, mapsize), AdditiveInt);
		AsGroundBlock();

	}

	public void AsGroundBlock()
	{
		MeshCol = setEdgeColor();
		tempMat.SetFloat("Vector1_B4969D56", HeighBlockValue);
		tempMat.SetFloat("Vector1_3FE5211C", this.HeightDirection);
		tempMat.SetFloat("Vector3_2D939FDE", 0.4f);
		tempMat.SetFloat("Vector1_B28DC083", -0.10f) ;
		tempMat.SetColor("Color_A8808C6D", grassColor);
		tempMat.SetColor("Color_3D31380E", emissionCol);
		tempMat.SetFloat("Vector1_BB62BD58", -1.4f);//1.5f
		tempMat.SetFloat("Vector1_803D8F42", -0.18f * 8);//Valeur plus petite pour BCP DE MAPSIZE et inverse
		MeshFilter meshFl = GetComponent<MeshFilter>();
		Mesh meshCopy = Mesh.Instantiate(meshFl.sharedMesh);

		mesh = meshFl.mesh = meshCopy;

		mesh.colors32 = MeshCol;
		meshRender.material = tempMat;
	}
	public void setOrderEdgeTop(List<int> nbPoint,float additiveInt) // pour avoir la list en vector des i du tableaux de verticesTop
	{
		//float tt = Random.Range(0, 0.9f);
		//HeighBlockValue = 0.262f;
		foreach (int edges in nbPoint)
		{

			EdgeList[edges] = 0.5f;

		

		}
		if (nbpoint.Count >= 4)
		{
			//nbpoint.Add(10);
			//EdgeList[10] = Random.Range(0.4f, 0.55f);
		}
		else if(nbpoint.Count >= 3)
		{
			EdgeList[10] = Random.Range(0.05f,0.2f);
		}
	}
	
	public Color32[] setEdgeColor()
	{
		colors = new Color32[vertices.Length];
		int PointLastElEment = EdgeList.Length;
		
		for (int w = 0; w < EdgeList.Length; w++)
		{
			colors[w] = Color.Lerp(Color.red, Color.green, EdgeList[w]);
			
		}
		return colors;
	}

}

