import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { genreApi, artistApi, trackApi, playlistApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function Dashboard() {
  const [stats, setStats] = useState({ genres: 0, artists: 0, tracks: 0, playlists: 0, avgBpm: 0 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadStats();
  }, []);

  async function loadStats() {
    try {
      setLoading(true);
      setError('');
      const [genres, artists, tracks, playlists] = await Promise.all([
        genreApi.getAll(),
        artistApi.getAll(),
        trackApi.getAll(),
        playlistApi.getAll()
      ]);
      const avgBpm = tracks.length > 0
        ? Math.round(tracks.reduce((sum, t) => sum + t.bpm, 0) / tracks.length)
        : 0;
      setStats({
        genres: genres.length,
        artists: artists.length,
        tracks: tracks.length,
        playlists: playlists.length,
        avgBpm
      });
    } catch (err) {
      setError('Não foi possível conectar à API. Verifique se o backend está rodando.');
    } finally {
      setLoading(false);
    }
  }

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="page">
      <h1 className="page-title">🎵 Dashboard</h1>
      <p className="page-subtitle">Visão geral do seu acervo musical</p>

      <div className="dashboard-grid">
        <Link to="/genres" className="dashboard-card card-genres">
          <div className="card-icon">🎸</div>
          <div className="card-value">{stats.genres}</div>
          <div className="card-label">Gêneros</div>
        </Link>
        <Link to="/artists" className="dashboard-card card-artists">
          <div className="card-icon">🎤</div>
          <div className="card-value">{stats.artists}</div>
          <div className="card-label">Artistas</div>
        </Link>
        <Link to="/tracks" className="dashboard-card card-tracks">
          <div className="card-icon">🎶</div>
          <div className="card-value">{stats.tracks}</div>
          <div className="card-label">Músicas</div>
        </Link>
        <Link to="/playlists" className="dashboard-card card-playlists">
          <div className="card-icon">📋</div>
          <div className="card-value">{stats.playlists}</div>
          <div className="card-label">Playlists</div>
        </Link>
        <div className="dashboard-card card-bpm">
          <div className="card-icon">💓</div>
          <div className="card-value">{stats.avgBpm}</div>
          <div className="card-label">BPM Médio</div>
        </div>
      </div>
    </div>
  );
}
