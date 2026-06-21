import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Dashboard from './pages/Dashboard';
import GenresPage from './pages/GenresPage';
import ArtistsPage from './pages/ArtistsPage';
import TracksPage from './pages/TracksPage';
import PlaylistsPage from './pages/PlaylistsPage';
import PlaylistDetailsPage from './pages/PlaylistDetailsPage';

export default function App() {
  return (
    <div className="app">
      <Navbar />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/genres" element={<GenresPage />} />
          <Route path="/artists" element={<ArtistsPage />} />
          <Route path="/tracks" element={<TracksPage />} />
          <Route path="/playlists" element={<PlaylistsPage />} />
          <Route path="/playlists/:id" element={<PlaylistDetailsPage />} />
        </Routes>
      </main>
    </div>
  );
}
