import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useState } from 'react';
import { User } from './types';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import Projects from './pages/Projects';
import Tasks from './pages/Tasks';
import Reports from './pages/Reports';
import Login from './pages/Login';

function App() {
  const [currentUser, setCurrentUser] = useState<User | null>({
    id: 1,
    name: 'Felipe de Melo Souza',
    email: 'felip-nho@hotmail.com',
    role: 'Manager',
    createdAt: new Date().toISOString()
  });

  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        {currentUser ? (
          <Layout currentUser={currentUser} onLogout={() => setCurrentUser(null)} children={undefined}>
            <Routes>
              <Route path="/" element={<Dashboard />} />
              <Route path="/projects" element={<Projects />} />
              <Route path="/tasks" element={<Tasks />} />
              <Route path="/reports" element={<Reports />} />
            </Routes>
          </Layout>
        ) : (
          <Login onLogin={setCurrentUser} />
        )}
      </div>
    </Router>
  );
}

export default App;
