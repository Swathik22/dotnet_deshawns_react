import { useState,useEffect } from "react"
import { deleteDog, getAllDogs } from "../services/DogService"
import { Link, useNavigate } from "react-router-dom"

export const ViewAllDogs=()=>{
    const[allDogsList,setAllDogsList]=useState([])
    const navigate=useNavigate()
        useEffect(()=>{
            getAllDogs().then(allDogs=>{
                setAllDogsList(allDogs)
            }
            )
        }      
    ,[])

    const handleNavigateToAddDog=()=>{
        navigate("/addDog")
    }

    const handleRemoveDog=(dogId)=>{
        deleteDog(dogId).then(
            navigate("/")
      
    )
    }

    return <>
    <h4>View All Dogs</h4>
    <div>   
        <button onClick={handleNavigateToAddDog}>Add Dog</button>    
    </div>
      <div> 
            {
            allDogsList.map(dog=>{                   
                return (
                    <div>
                        <div key={dog.id}><Link to={`/${dog.id}`} style={{marginRight: '25px'}}>{dog.name}</Link><button onClick={() => handleRemoveDog(dog.id)}>Remove</button></div>
                        </div>
                      )             
            }
                
            )
            }
     </div>
    </>
}