import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import { BrowserRouter, Routes, Route, Outlet } from "react-router-dom";
import reportWebVitals from "./reportWebVitals";
import Home from "./Home";
import { ViewAllDogs } from "./components/ViewAllDogs";
import { ViewDogDetails } from "./components/ViewDogDetails";
import { AddDog } from "./components/AddDog";
import { ViewAllWalkers } from "./components/ViewAllwalkers";
import { AssignDogToWalker } from "./components/AssignDogToWalker";
import { AddCity } from "./components/AddCity";
import { UpdateWalker } from "./components/UpdateWalker";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <BrowserRouter>
    <Routes>
      {/* <Route path="/" element={<App />}> */}
        {/* <Route index element={<Home />} /> */}
    <Route 
      path="/" 
      element={
        <>   
          <App/>          
        </>
      }
    >      
      <Route index element={<ViewAllDogs/>}/>  
      <Route path=":dogId" element={<ViewDogDetails/>}  />  
      <Route path="addDog" element={<AddDog/>}/>
      <Route path="walkers" element={<ViewAllWalkers/>}/>
      <Route path="walkers/:walkerId" element={<AssignDogToWalker/>}/>
      <Route path="city" element={<AddCity/>}/>
      <Route path="updateWalker/:walkerId" element={<UpdateWalker/>}/>
    </Route>
    </Routes>
  </BrowserRouter>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
