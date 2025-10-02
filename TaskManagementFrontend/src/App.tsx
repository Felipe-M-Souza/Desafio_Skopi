import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useState } from 'react';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import Projects from './pages/Projects';
import Tasks from './pages/Tasks';
import Reports from './pages/Reports';
import Login from './pages/Login';

function App() {
  const [currentUser, setCurrentUser] = useState({
    id: 1,
    name: 'João Silva',
    email: 'joao@email.com',
    role: 'Manager' as const
  });

  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        {currentUser ? (
          <Layout currentUser={currentUser} onLogout={() => setCurrentUser(null)}>
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
