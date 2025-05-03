using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class PieceReagent : Reagent
    {
        [SerializeField] private GameObject _piecePrefab;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _checkSpawnRadius = .3f;
        [SerializeField] private LayerMask _reagentLayer;
        
        private List<GameObject> _pieces = new();
        
        protected override void UpdateVisual(int newValue, int oldValue)
        {
            if (newValue > _pieces.Count)
                CreateNewObjects(newValue - _pieces.Count);

            if (newValue > oldValue)
            {
                int objectsToSpawn = newValue - oldValue;
                List<GameObject> piecesToSpawn = new List<GameObject>();

                for (int i = 0; i < objectsToSpawn; i++)
                    piecesToSpawn.Add(_pieces[oldValue + i]);
                
                StartCoroutine(SpawnObjects(piecesToSpawn));
            }
            else if (newValue < oldValue)
            {
                int piecesToTurnOff = oldValue - newValue;
                for (int i = 0; i < piecesToTurnOff; i++)
                {
                    var activePieces = _pieces.Where(piece => piece.activeSelf).ToList();
                    if (activePieces.Count == 0) 
                        break;

                    var randomPiece = activePieces[Random.Range(0, activePieces.Count)];

                    randomPiece.SetActive(false);

                    _pieces.Remove(randomPiece);
                    _pieces.Add(randomPiece);
                }
            }
        }

        private void CreateNewObjects(int piecesCount)
        {
            for (int i = 0; i < piecesCount; i++)
            {
                var piece = Instantiate(_piecePrefab, transform);
                piece.SetActive(false);
                _pieces.Add(piece);
            }
        }

        private IEnumerator SpawnObjects(List<GameObject> piecesToSpawn)
        {
            int spawnTry = 0;
            
            for (int i = 0; i < piecesToSpawn.Count; i++)
            {
                bool spawned = false;

                Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
                
                if (!Physics.CheckSphere(spawnPoint.position, _checkSpawnRadius, _reagentLayer) && !spawned)
                {
                    piecesToSpawn[i].transform.position = spawnPoint.position;
                    piecesToSpawn[i].SetActive(true);
                    spawned = true;
                    spawnTry = 0;
                }

                yield return new WaitForSeconds(.1f);

                
                if (!spawned)
                {
                    i--;
                    spawnTry++;
                }
                if (spawnTry == 10)
                {
                    Debug.LogError($"can't spawn piece item! {name}");
                    break;
                }
            }
        }
    }
}