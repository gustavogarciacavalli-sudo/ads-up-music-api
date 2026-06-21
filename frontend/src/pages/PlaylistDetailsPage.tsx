import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Playlist, PlaylistSummary } from '../types/Playlist';
import { Track } from '../types/Track';
import { playlistApi, trackApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function PlaylistDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const playlistId = Number(id);

  const [playlist, setPlaylist] = useState<Playlist | null>(null);
  const [playlistTracks, setPlaylistTracks] = useState<Track[]>([]);
  const [allTracks, setAllTracks] = useState<Track[]>([]);
  const [summary, setSummary] = useState<PlaylistSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [selectedTrackId, setSelectedTrackId] = useState<number>(0);

  useEffect(() => { loadData(); }, [playlistId]);

  async function loadData() {
    try {
      setLoading(true); setError('');
      const [p, pt, at, s] = await Promise.all([
        playlistApi.getById(playlistId),
        playlistApi.getTracks(playlistId),
        trackApi.getAll(),
        playlistApi.getSummary(playlistId)
      ]);
      setPlaylist(p); setPlaylistTracks(pt); setAllTracks(at); setSummary(s);
    } catch { setError('Erro ao carregar detalhes da playlist.'); }
    finally { setLoading(false); }
  }

  async function handleAddTrack() {
    if (!selectedTrackId) return;
    try {
      await playlistApi.addTrack(playlistId, selectedTrackId);
      setSelectedTrackId(0);
      loadData();
    } catch (err: any) { setError(err.message || 'Erro ao adicionar música.'); }
  }

  async function handleRemoveTrack(trackId: number) {
    try {
      await playlistApi.removeTrack(playlistId, trackId);
      loadData();
    } catch (err: any) { setError(err.message || 'Erro ao remover música.'); }
  }

  const availableTracks = allTracks.filter(t => !t.playlistId || t.playlistId !== playlistId);

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;
  if (!playlist) return <ErrorMessage message="Playlist não encontrada." />;

  return (
    <div className="page">
      <Link to="/playlists" className="back-link">← Voltar para Playlists</Link>

      <div className="playlist-detail-header">
        <div>
          <h1 className="page-title">📋 {playlist.name}</h1>
          <p className="page-subtitle">{playlist.description}</p>
          {playlist.mood && <span className="mood-badge">{playlist.mood}</span>}
        </div>
      </div>

      {summary && (
        <div className="summary-grid">
          <div className="summary-item">
            <span className="summary-value">{summary.trackCount}</span>
            <span className="summary-label">Músicas</span>
          </div>
          <div className="summary-item">
            <span className="summary-value">{summary.averageBpm}</span>
            <span className="summary-label">BPM Médio</span>
          </div>
          <div className="summary-item">
            <span className="summary-value">{summary.totalDuration}</span>
            <span className="summary-label">Duração Total</span>
          </div>
          <div className="summary-item">
            <span className="summary-value">{summary.artists.length}</span>
            <span className="summary-label">Artistas</span>
          </div>
          <div className="summary-item">
            <span className="summary-value">{summary.genres.length}</span>
            <span className="summary-label">Gêneros</span>
          </div>
        </div>
      )}

      {summary && (summary.artists.length > 0 || summary.genres.length > 0) && (
        <div className="summary-details">
          {summary.artists.length > 0 && (
            <div className="summary-tags">
              <strong>Artistas:</strong>
              {summary.artists.map((a, i) => <span key={i} className="tag">{a}</span>)}
            </div>
          )}
          {summary.genres.length > 0 && (
            <div className="summary-tags">
              <strong>Gêneros:</strong>
              {summary.genres.map((g, i) => <span key={i} className="tag tag-genre">{g}</span>)}
            </div>
          )}
        </div>
      )}

      <div className="add-track-section">
        <h3>Adicionar Música à Playlist</h3>
        <div className="add-track-row">
          <select value={selectedTrackId} onChange={e => setSelectedTrackId(Number(e.target.value))}>
            <option value={0}>Selecione uma música...</option>
            {availableTracks.map(t => (
              <option key={t.id} value={t.id}>{t.title} - {t.artistName}</option>
            ))}
          </select>
          <button className="btn btn-primary" onClick={handleAddTrack} disabled={!selectedTrackId}>+ Adicionar</button>
        </div>
      </div>

      <h3>Músicas na Playlist ({playlistTracks.length})</h3>
      {playlistTracks.length === 0 ? (
        <p className="empty-state">Nenhuma música nesta playlist. Adicione uma acima!</p>
      ) : (
        <div className="table-container">
          <table className="data-table">
            <thead>
              <tr><th>Título</th><th>Artista</th><th>BPM</th><th>Duração</th><th>Ação</th></tr>
            </thead>
            <tbody>
              {playlistTracks.map(t => (
                <tr key={t.id}>
                  <td>{t.title}</td>
                  <td>{t.artistName}</td>
                  <td><span className="bpm-badge">{t.bpm}</span></td>
                  <td>{t.duration}</td>
                  <td>
                    <button className="btn btn-sm btn-delete" onClick={() => handleRemoveTrack(t.id)}>🗑️ Remover</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
