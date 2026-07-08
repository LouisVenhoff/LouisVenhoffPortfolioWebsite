import { ReactNode } from 'react'
import UnderConstruction from './pages/underConstruction/underConstruction'
import Header from './staticContent/header/header'
import NavButton from './staticContent/header/navButton/navButton'
import { BrowserRouter as Router, Route, Routes, Outlet, Navigate } from 'react-router-dom'
import Home from './pages/home/home'
import Project from './pages/project/project'
import Projects from './pages/projects/projects'
import About from './pages/about/about'

function App() {
  
  
  
  const renderHeader = ():ReactNode => {
    return(
      <>
        <Header>
          <NavButton title="Home" target={'home'} />
          <NavButton title="Projekte" target={'projects'} />
          <NavButton title="Über mich" target={'about'} />
        </Header>
        <Outlet />
      </>
    );
  }
  
  
  
  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={renderHeader()}>
            <Route index element={<Navigate to="/home" replace />} />
            <Route path="construction" element={<UnderConstruction/>}/>
            <Route path="home" element={<Home />}/>
            <Route path="project/:id" element={<Project />} />
            <Route path="projects" element={<Projects />}/>
            <Route path="about" element={<About />}/>
          </Route>
        </Routes>
      </Router>
    </>
  )
}

export default App
