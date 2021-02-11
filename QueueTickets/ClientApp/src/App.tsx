import React, { Component } from 'react';
import { Route } from 'react-router';
import SpecialistControlPanelPage from './components/SpecialistControlPanelPage';
import './custom.css'



const App: React.FC = props => {
    return (
        <div className="App">
            {SpecialistControlPanelPage()}
        </div>
    )
}

export default App